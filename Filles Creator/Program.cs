using System;
using System.IO;
using System.Text;
using System.Text.Json;
using iTextSharp.text;               // For PDF
using iTextSharp.text.pdf;           // For PDF
using DocumentFormat.OpenXml.Packaging;
using Wp = DocumentFormat.OpenXml.Wordprocessing;  // Alias for Wordprocessing

class Program
{
    private static readonly Random random = new Random();
    private static readonly string outputDir = @"C:\Users\DELL\Documents\Client_Path2";

    static void Main()
    {
        if (!Directory.Exists(outputDir))
            Directory.CreateDirectory(outputDir);

        string[] extensions = { "txt", "pdf", "docx", "csv", "json", "xml", "html", "jpg", "png" };

        for (int i = 5001; i <= 20000; i++)
        {
            string ext = extensions[random.Next(extensions.Length)];
            string filePath = Path.Combine(outputDir, $"file_{i}.{ext}");

            switch (ext)
            {
                case "txt": CreateTxt(filePath); break;
                case "pdf": CreatePdf(filePath); break;
                case "docx": CreateDocx(filePath); break;
                case "csv": CreateCsv(filePath); break;
                case "json": CreateJson(filePath); break;
                case "xml": CreateXml(filePath); break;
                case "html": CreateHtml(filePath); break;
                case "jpg": CreateBinary(filePath); break;
                case "png": CreateBinary(filePath); break;
            }

            if (i % 1000 == 0)
                Console.WriteLine($"Created {i} files...");
        }

        Console.WriteLine("✅ 1 lakh sample files generated successfully!");
    }

    static string RandomText(int length = 50)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var sb = new StringBuilder();
        for (int i = 0; i < length; i++)
            sb.Append(chars[random.Next(chars.Length)]);
        return sb.ToString();
    }

    static void CreateTxt(string path) =>
        File.WriteAllText(path, RandomText(200));

    static void CreatePdf(string path)
    {
        using var fs = new FileStream(path, FileMode.Create);
        using var doc = new iTextSharp.text.Document();
        PdfWriter.GetInstance(doc, fs);
        doc.Open();
        doc.Add(new iTextSharp.text.Paragraph(RandomText(100)));
        doc.Close();
    }

    static void CreateDocx(string path)
    {
        using var doc = WordprocessingDocument.Create(path, DocumentFormat.OpenXml.WordprocessingDocumentType.Document);
        var mainPart = doc.AddMainDocumentPart();
        mainPart.Document = new Wp.Document();
        var body = mainPart.Document.AppendChild(new Wp.Body());
        body.AppendChild(new Wp.Paragraph(new Wp.Run(new Wp.Text(RandomText(100)))));
    }

    static void CreateCsv(string path)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Id,Value");
        for (int i = 1; i <= 10; i++)
            sb.AppendLine($"{i},{RandomText(10)}");
        File.WriteAllText(path, sb.ToString());
    }

    static void CreateJson(string path)
    {
        var obj = new { id = random.Next(1, 1000), text = RandomText(20) };
        string json = JsonSerializer.Serialize(obj);
        File.WriteAllText(path, json);
    }

    static void CreateXml(string path)
    {
        string xml = $"<root><id>{random.Next(1, 1000)}</id><text>{RandomText(20)}</text></root>";
        File.WriteAllText(path, xml);
    }

    static void CreateHtml(string path)
    {
        string html = $"<html><body><p>{RandomText(50)}</p></body></html>";
        File.WriteAllText(path, html);
    }

    static void CreateBinary(string path)
    {
        byte[] bytes = new byte[1024]; // 1KB fake binary
        random.NextBytes(bytes);
        File.WriteAllBytes(path, bytes);
    }
}
