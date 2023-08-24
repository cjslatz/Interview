using CsvHelper;
using CsvHelper.Configuration;
using DocumentManager.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System;
using System.Globalization;
using System.IO;

namespace DocumentManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentController : ControllerBase
    {
        public static string docsFolder = Path.Combine(Directory.GetCurrentDirectory(), "ClientApp\\public\\Docs");
        public static string documentCsvFilePath = Path.Combine(docsFolder, "Documents.csv");

        private List<CsvData> ReadCsvFile(string filePath)
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    return csv.GetRecords<CsvData>().ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading CSV file: {ex.Message}");
                return new List<CsvData>();
            }
        }

        private void WriteCsvFile(string filePath, List<CsvData> csvDataList)
        {
            try
            {
                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    csv.WriteRecords(csvDataList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing CSV file: {ex.Message}");
                throw;
            }
        }

        [HttpGet]
        public ActionResult<List<CsvData>> Get()
        {
            List<CsvData> csvDataList;
            using (var reader = new StreamReader(documentCsvFilePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csvDataList = csv.GetRecords<CsvData>().ToList();
            }

            return csvDataList;
        }

        [HttpDelete]
        public ActionResult Delete([FromQuery] string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(path))
                {
                    var filePathToDelete = Path.Combine(Directory.GetCurrentDirectory(), path); 
                    if (System.IO.File.Exists(filePathToDelete))
                    {
                        List<CsvData> csvDataList;
                        using (var reader = new StreamReader(documentCsvFilePath))
                        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                        {
                            csvDataList = csv.GetRecords<CsvData>().ToList();
                        }

                        var csvLines = System.IO.File.ReadAllLines(documentCsvFilePath).ToList();

                        csvLines.RemoveAll(line => line.Contains(path));

                        System.IO.File.WriteAllLines(documentCsvFilePath, csvLines);

                        System.IO.File.Delete(filePathToDelete);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest("Invalid file path");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("upload")]
        public async Task<ActionResult> Upload([FromForm] IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(docsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var newCsvEntry = new CsvData
                    {
                        Name = fileName,
                        Path = filePath, 
                        Category = "SomeCategory"
                    };

                    var csvDataList = ReadCsvFile(documentCsvFilePath);

                    csvDataList.Add(newCsvEntry);

                    WriteCsvFile(documentCsvFilePath, csvDataList);

                    return Ok("File uploaded successfully.");
                }
                else
                {
                    return BadRequest("Invalid file.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
