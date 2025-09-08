using System.Text.Json;
using System.Net.Sockets;
using IniParser;
using IniParser.Model;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;

namespace RobustAccessDbSync
{
    class Program
    {
        static string DRIVE_LETTER = "X:";
        static bool _syncRunning = true;
        static bool _isOnline = true;
        static DateTime _lastOnlineTime = DateTime.MinValue;
        static int _syncCycleWaitMinutes = 1;
        static Stopwatch _cycleTimer = new Stopwatch();
        static DateTime _nextSyncTime = DateTime.Now;
        static string clientPath;
        static string serverPath;
        static string SERVER_IP;
        static string SHARE_NAME;
        static string USERNAME;
        static string PASSWORD;
        static string? rememberedClientPath = null;
        static List<string> Client_Folders = [];
        static string syncMetaFile = "sync_metadata.json";
        static DateTime _lastSyncTime = DateTime.MinValue;
        static int Count = 0;

        // Enhanced metadata tracking
        class SyncMetadata
        {
            public DateTime LastSyncTime { get; set; } = DateTime.MinValue;
            public Dictionary<string, FileMetadata> Files { get; set; } = new();
        }

        class FileMetadata
        {
            public DateTime LastModified { get; set; }
            public long FileSize { get; set; }
            public string FilePath { get; set; } = string.Empty;
        }

        static SyncMetadata _syncMetadata = new();

        // Parallel processing settings
        static int _maxDegreeOfParallelism = Environment.ProcessorCount;
        static int _batchSize = 1000;

        // ---------- DRY Helpers ----------
        static string PromptUntilValid(string message, Func<string, bool> validator, string errorMessage, bool isPassword = false)
        {
            string input;
            do
            {
                Console.Write(message);
                input = isPassword ? ReadPassword() : Console.ReadLine();
                if (!validator(input))
                {
                    Console.WriteLine(errorMessage);
                    input = string.Empty;
                }
            } while (string.IsNullOrWhiteSpace(input));
            return input;
        }

        static void GetServerCredentials()
        {
            while (true)
            {
                USERNAME = PromptUntilValid("Enter USERNAME: ", s => !string.IsNullOrWhiteSpace(s), "USERNAME cannot be empty");
                PASSWORD = PromptUntilValid("Enter PASSWORD: ", s => !string.IsNullOrWhiteSpace(s), "PASSWORD cannot be empty", true);

                Console.WriteLine("\nPress Enter to continue or type 'r' to re-enter:");
                string input = Console.ReadLine()?.Trim().ToLower();
                if (string.IsNullOrEmpty(input)) break;
                if (input != "r") Console.WriteLine("Invalid input. Re-entering...\n");
            }
        }

        static void GetClientsServerPath()
        {
            Console.Title = "File Synchronization Tool";
            Console.CursorVisible = false;
            PrintHeader();
            ShowGameStyleLoader("Initializing File Synchronization Tool", 20);

            while (true)
            {
                clientPath = PromptUntilValid("Enter Client Root Path: ", Directory.Exists, "Invalid client path");

                serverPath = PromptUntilValid("Enter Server Path: ", s => !string.IsNullOrWhiteSpace(s), "Server path cannot be empty");
                var serverParts = serverPath.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                if (serverParts.Length < 2)
                {
                    PrintError("Invalid server path format. Expected: \\\\server\\share\\path");
                    continue;
                }

                SERVER_IP = serverParts[0];
                SHARE_NAME = serverParts[1];

                Console.WriteLine("\nPress Enter to continue or type 'r' to re-enter:");
                string input = Console.ReadLine()?.Trim().ToLower();
                if (string.IsNullOrEmpty(input)) break;
                if (input != "r") Console.WriteLine("Invalid input. Re-entering...\n");
            }
        }

        static void GetClientPathCredentials()
        {
            Client_Folders = Enumerable.Range(1, int.MaxValue)
                .Select(i =>
                {
                    Console.Write($"Enter client folder path #{i} (leave blank to stop): ");
                    var path = Console.ReadLine();
                    return string.IsNullOrWhiteSpace(path) ? null : path;
                })
                .TakeWhile(path => path != null)
                .Where(Directory.Exists)
                .ToList();

            if (!Client_Folders.Any())
                Console.WriteLine("Warning: No folders entered for sync.");
        }

        // Fast directory enumeration
        static IEnumerable<string> EnumerateFilesFast(string path, string searchPattern = "*")
        {
            var files = new ConcurrentBag<string>();
            var directories = new ConcurrentStack<string>();
            directories.Push(path);

            while (directories.TryPop(out var currentDir))
            {
                try
                {
                    foreach (var f in Directory.EnumerateFiles(currentDir, searchPattern)) files.Add(f);
                    foreach (var d in Directory.EnumerateDirectories(currentDir)) directories.Push(d);
                }
                catch { }
            }
            return files;
        }

        static void LoadSyncMetadata()
        {
            try
            {
                string metadataPath = Path.Combine(clientPath, syncMetaFile);
                if (File.Exists(metadataPath))
                {
                    string json = File.ReadAllText(metadataPath);
                    _syncMetadata = JsonSerializer.Deserialize<SyncMetadata>(json) ?? new SyncMetadata();
                    _lastSyncTime = _syncMetadata.LastSyncTime;
                    PrintSuccess($"Loaded sync metadata. Last sync: {_lastSyncTime:yyyy-MM-dd HH:mm:ss}");
                    PrintInfo($"Tracked files: {_syncMetadata.Files.Count:N0}");
                }
                else
                {
                    PrintInfo("No previous sync metadata found. Starting fresh.");
                }
            }
            catch (Exception ex)
            {
                PrintError($"Error loading sync metadata: {ex.Message}");
                _syncMetadata = new SyncMetadata();
            }
        }

        static void SaveSyncMetadata()
        {
            try
            {
                _syncMetadata.LastSyncTime = DateTime.UtcNow;
                string metadataPath = Path.Combine(clientPath, syncMetaFile);
                string json = JsonSerializer.Serialize(_syncMetadata, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(metadataPath, json);
            }
            catch (Exception ex)
            {
                PrintError($"Error saving sync metadata: {ex.Message}");
            }
        }

        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        static async Task Main()
        {
            var parser = new FileIniDataParser();
            IniData data = null;
            const string pointerFile = "last_path.txt";

            try
            {
                if (File.Exists(pointerFile))
                    rememberedClientPath = File.ReadAllText(pointerFile)?.Trim();

                if (!string.IsNullOrEmpty(rememberedClientPath))
                {
                    string iniPath = Path.Combine(rememberedClientPath, "config.ini");
                    if (File.Exists(iniPath))
                    {
                        data = parser.ReadFile(iniPath);
                        USERNAME = data["Credentials"]["Username"];
                        PASSWORD = data["Credentials"]["Password"];
                        SERVER_IP = data["Server"]["IP"];
                        SHARE_NAME = data["Server"]["Share"];
                        serverPath = data["Server"]["Path"];
                        clientPath = data["Client"]["Path"];

                        int index = 1;
                        while (data["folder"].ContainsKey($"Path{index}"))
                        {
                            Client_Folders.Add(data["folder"][$"Path{index}"]);
                            index++;
                        }
                        Console.WriteLine("Loaded saved configuration.");
                    }
                }

                if (data == null)
                {
                    Console.WriteLine("No saved configuration found. Please enter details.");
                    GetClientsServerPath();
                    GetServerCredentials();
                    GetClientPathCredentials();

                    data = new IniData();
                    data.Sections.AddSection("folder");
                    data["Credentials"]["Username"] = USERNAME;
                    data["Credentials"]["Password"] = PASSWORD;
                    data["Server"]["IP"] = SERVER_IP;
                    data["Server"]["Share"] = SHARE_NAME;
                    data["Server"]["Path"] = serverPath;
                    data["Client"]["Path"] = clientPath;

                    for (int i = 0; i < Client_Folders.Count; i++)
                        data["folder"][$"Path{i + 1}"] = Client_Folders[i];

                    parser.WriteFile(Path.Combine(clientPath, "config.ini"), data);
                }

                LoadSyncMetadata();
                Console.WriteLine("Ready to sync using loaded configuration.");
            }
            catch (Exception ex)
            {
                PrintError(" ERROR: " + ex.Message);
            }

            File.WriteAllText(pointerFile, clientPath);

            PrintSuccess("\nStarting file synchronization...");
            PrintInfo("Press 'S' to stop, 'R' to restart, 'Q' to quit.\n");

            var syncTask = Task.Run(() => ContinuousFileSync());

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Q)
                    {
                        if (_syncRunning)
                        {
                            _syncRunning = false;
                            PrintWarning("Stopping synchronization...");
                            await syncTask;
                        }
                        break;
                    }
                    else if (key == ConsoleKey.S && _syncRunning)
                    {
                        _syncRunning = false;
                        PrintWarning("Stopping synchronization...");
                        await syncTask;
                        PrintInfo("Stopped. Press 'R' to restart or 'Q' to quit.");
                    }
                    else if (key == ConsoleKey.R)
                    {
                        if (_syncRunning)
                        {
                            _syncRunning = false;
                            PrintWarning("Restarting synchronization...");
                            await syncTask;
                        }
                        _syncRunning = true;
                        syncTask = Task.Run(() => ContinuousFileSync());
                    }
                }
                await Task.Delay(100);
            }

            PrintInfo("\nExited. Press any key to close.");
            Console.CursorVisible = true;
            Console.ReadKey();
        }

        static string ReadPassword()
        {
            var sb = new StringBuilder();
            ConsoleKeyInfo key;
            while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                if (key.Key == ConsoleKey.Backspace && sb.Length > 0)
                {
                    sb.Length--; Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    sb.Append(key.KeyChar); Console.Write("*");
                }
            }
            Console.WriteLine();
            return sb.ToString();
        }

        static bool RunCommand(string command, bool showOutput = true)
        {
            try
            {
                ProcessStartInfo psi = new("cmd.exe", "/c " + command)
                {
                    RedirectStandardOutput = !showOutput,
                    RedirectStandardError = !showOutput,
                    UseShellExecute = false,
                    CreateNoWindow = !showOutput
                };
                using var proc = Process.Start(psi);
                proc.WaitForExit();
                return proc.ExitCode == 0;
            }
            catch (Exception ex)
            {
                PrintError("Command failed: " + ex.Message);
                return false;
            }
        }

        // ---------- Optimized Sync ----------
        static void SyncFiles(string sourceFolder, string targetFolder, string logFile, string direction, bool isFullServerToClient)
        {
            if (!Directory.Exists(sourceFolder)) return;

            var allFiles = EnumerateFilesFast(sourceFolder).ToArray();
            PrintInfo($"Scanning {Path.GetFileName(sourceFolder)}: {allFiles.Length:N0} files");

            var newMetadata = allFiles.AsParallel()
                .WithDegreeOfParallelism(_maxDegreeOfParallelism)
                .Select(file =>
                {
                    var fi = new FileInfo(file);
                    var relative = Path.GetRelativePath(sourceFolder, file);
                    return new KeyValuePair<string, FileMetadata>(relative, new FileMetadata
                    {
                        LastModified = fi.LastWriteTimeUtc,
                        FileSize = fi.Length,
                        FilePath = relative
                    });
                })
                .Where(kv => kv.Key != null)
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            var changedFiles = newMetadata
                .Where(kv => !_syncMetadata.Files.TryGetValue(kv.Key, out var old) ||
                             kv.Value.LastModified > old.LastModified ||
                             kv.Value.FileSize != old.FileSize )
                .Select(kv => Path.Combine(sourceFolder, kv.Key))
                .ToList();

            var deletedFiles = _syncMetadata.Files.Keys
             .Where(oldKey => !newMetadata.ContainsKey(oldKey))
                .ToList();

            PrintInfo($"Changed files: {changedFiles.Count:N0}");
            
            foreach (var batch in changedFiles.Chunk(_batchSize))
            {
                batch.AsParallel().WithDegreeOfParallelism(_maxDegreeOfParallelism).ForAll(src =>
                {
                    try
                    {
                        string relative = Path.GetRelativePath(sourceFolder, src);
                        string dest = Path.Combine(targetFolder, relative);
                        Directory.CreateDirectory(Path.GetDirectoryName(dest));
                        File.Copy(src, dest, true);
                        Interlocked.Increment(ref Count);
                        PrintSuccess($"[✓] Copied: {relative} {direction}");
                     
                    }
                    catch (Exception ex)
                    {
                        PrintError($"Error copying {src}: {ex.Message}");
                    }
                });
            }
            if (changedFiles.Count > 0) {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                var logLines = new List<string>
        {
            $"[{timestamp}]",
            $"Sync = Files",
            $"changes = {changedFiles.Count}",
            $"direction = {direction}",
            ""
        };
                File.AppendAllLines(logFile, logLines);
            }
            // here we update all data because of deletion inconsistency 
            _syncMetadata.Files = new Dictionary<string, FileMetadata>(newMetadata);
            SaveSyncMetadata();
        }

        static void SyncFilesBothDirections()
        {
            try
            {
                if (!_syncRunning) return;

                string logFile = Path.Combine(clientPath, "Configlog.ini");
                var excludeList = new List<string>();

                PrintInfo($"Starting sync (since {_lastSyncTime:yyyy-MM-dd HH:mm:ss})");

                foreach (var clientFolder in Client_Folders.Where(Directory.Exists))
                {
                    //string name = Path.GetFileName(clientFolder);

                    //string serverFolder = Path.Combine(serverPath, name);
                    var serverFolder = serverPath;
                   if (!Directory.Exists(serverFolder))
                    {
                        Directory.CreateDirectory(serverFolder);
                        PrintSuccess($"[+] Created server folder: {serverFolder}");
                    }
                    SyncFiles(clientFolder, serverFolder, logFile, "ClientToServer", false);
                    SyncFiles(serverFolder, clientFolder, logFile, "ServerToClient", false);
                }
            }
            catch (Exception ex)
            {
                PrintError($"Sync error: {ex.Message}");
            }
        }

        static async Task ContinuousFileSync()
        {
            while (_syncRunning)
            {
                try
                {
                    _cycleTimer.Restart();
                    PrintInfo($"Starting sync cycle at {DateTime.Now:T}");

                    _isOnline = CheckNetworkConnection(SERVER_IP);
                    if (_isOnline)
                    {
                        if (_lastOnlineTime == DateTime.MinValue) PrintSuccess("Connection restored");
                        _lastOnlineTime = DateTime.Now;

                        RunCommand($"net use {DRIVE_LETTER} /delete", false);
                        string connectCmd = $"net use {DRIVE_LETTER} \\\\{SERVER_IP}\\{SHARE_NAME} /user:{USERNAME} {PASSWORD} /persistent:no";
                        if (RunCommand(connectCmd))
                        {
                            SyncFilesBothDirections();
                            RunCommand($"net use {DRIVE_LETTER} /delete", false);
                        }
                        else PrintError("Failed to connect to shared folder");

                        _cycleTimer.Stop();
                        PrintSuccess($"Cycle completed in {_cycleTimer.Elapsed.TotalSeconds:0.00}s");
                    }
                    else if (_lastOnlineTime != DateTime.MinValue)
                    {
                        PrintWarning("Connection lost - offline mode");
                        _lastOnlineTime = DateTime.MinValue;
                    }

                    _nextSyncTime = DateTime.Now.AddMinutes(_syncCycleWaitMinutes);
                    PrintInfo($"Next sync at {_nextSyncTime:T}");

                    while (DateTime.Now < _nextSyncTime && _syncRunning)
                    {
                        TimeSpan remaining = _nextSyncTime - DateTime.Now;
                        Console.Write($"\rWaiting {remaining.Minutes}:{remaining.Seconds:00}...");
                        await Task.Delay(1000);
                    }
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    PrintError($"Cycle error: {ex.Message}");
                }
            }
        }

        static bool CheckNetworkConnection(string ip)
        {
            try
            {
                using var tcp = new TcpClient();
                var result = tcp.BeginConnect(ip, 445, null, null);
                var success = result.AsyncWaitHandle.WaitOne(2000);
                if (tcp.Connected) tcp.EndConnect(result);
                return success;
            }
            catch { return false; }
        }

        // ---------- Console Helpers ----------
        static void PrintHeader() { Console.ForegroundColor = ConsoleColor.Cyan; Console.WriteLine("\nFile Synchronization Tool\n-------------------------"); Console.ResetColor(); }
        static void PrintInfo(string msg) { Console.ForegroundColor = ConsoleColor.Cyan; Console.WriteLine($"[{DateTime.Now:T}] {msg}"); Console.ResetColor(); }
        static void PrintSuccess(string msg) { Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine($"[{DateTime.Now:T}] {msg}"); Console.ResetColor(); }
        static void PrintWarning(string msg) { Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine($"[{DateTime.Now:T}] {msg}"); Console.ResetColor(); }
        static void PrintError(string msg) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"[{DateTime.Now:T}] {msg}"); Console.ResetColor(); }

        static void ShowGameStyleLoader(string message, int steps)
        {
            Console.Write(message + " ");
            int width = 30;
            for (int i = 0; i <= steps; i++)
            {
                double pct = (double)i / steps;
                string bar = new string('█', (int)(pct * width)).PadRight(width, '-');
                Console.Write($"\r{message} [{bar}] {pct * 100:0}%");
                Thread.Sleep(20);
            }
            Console.WriteLine();
        }
    }
}
