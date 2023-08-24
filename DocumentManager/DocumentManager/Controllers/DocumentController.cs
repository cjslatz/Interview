using CsvHelper;
using CsvHelper.Configuration;
using DocumentManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace DocumentManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly string docsFolder;
        private readonly string publicFolder;
        private readonly string documentCsvFilePath;

        public DocumentController()
        {
            docsFolder = Path.Combine(Directory.GetCurrentDirectory(), "ClientApp\\public\\Docs");
            publicFolder = Path.Combine(Directory.GetCurrentDirectory(), "ClientApp\\public");
            documentCsvFilePath = Path.Combine(docsFolder, "Documents.csv");
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
                    var filePathToDelete = Path.Combine(publicFolder, path);
                    if (System.IO.File.Exists(filePathToDelete))
                    {
                        var csvDataList = ReadCsvFile();

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
                        Path = Path.Combine("Docs\\", fileName),
                        Category = "SomeCategory"
                    };

                    var csvDataList = ReadCsvFile();

                    csvDataList.Add(newCsvEntry);

                    WriteCsvFile(csvDataList);

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

        private List<CsvData> ReadCsvFile()
        {
            return CsvHelperExtensions.ReadCsvFile(documentCsvFilePath);
        }

        private void WriteCsvFile(List<CsvData> csvDataList)
        {
            CsvHelperExtensions.WriteCsvFile(documentCsvFilePath, csvDataList);
        }
    }
}
