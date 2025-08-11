using ExcelDataReader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

public class DistrictInfo
{
    public string Manager { get; set; }
    public List<string> DistrictNumber { get; set; } = new List<string>();
}

public class RegionInfo
{
    public string RegionName { get; set; }
    public Dictionary<string, DistrictInfo> Districts { get; set; } = new Dictionary<string, DistrictInfo>();
}

class Program
{
    static void Main(string[] args)
    {
        string excelFilePath = "C:\\Projects\\ExcelToJson\\region.xlsx";
        var allRegions = new List<RegionInfo>();

        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using (var fileStream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read))
        using (var excelReader = ExcelReaderFactory.CreateReader(fileStream))
        {
            RegionInfo currentRegion = null;
            DistrictInfo currentDistrict = null;
            int districtNumber = 0;
            bool isFirstRow = true;

            while (excelReader.Read())
            {
                if (excelReader.GetValue(0) == null || isFirstRow)
                {
                    isFirstRow = false;
                    continue;
                }

                string firstColumn = excelReader.GetValue(0).ToString();

                if (firstColumn.StartsWith("Region"))
                {
                    currentRegion = new RegionInfo
                    {
                        RegionName = firstColumn,
                        Districts = new Dictionary<string, DistrictInfo>()
                    };
                    allRegions.Add(currentRegion);
                    districtNumber = 0;
                }
                else if (firstColumn.StartsWith("Dist"))
                {
                    districtNumber++;
                    string districtKey = "d" + districtNumber;

                    string managerName = excelReader.GetValue(5)?.ToString();
                    if (!string.IsNullOrEmpty(managerName) && !managerName.Contains("Stores"))
                    {
                        currentDistrict = new DistrictInfo
                        {
                            Manager = managerName
                        };

                        if (currentRegion != null)
                        {
                            currentRegion.Districts[districtKey] = currentDistrict;
                        }
                    }
                }
                else if (int.TryParse(firstColumn, out _))
                {
                    if (currentDistrict != null)
                    {
                        currentDistrict.DistrictNumber.Add(firstColumn);
                    }
                }
            }
        }

        // Remove any empty regions
        allRegions.RemoveAll(r => string.IsNullOrEmpty(r.RegionName) || r.Districts.Count == 0);

        string jsonResult = JsonConvert.SerializeObject(allRegions, Formatting.Indented);
        Console.WriteLine(jsonResult);

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}