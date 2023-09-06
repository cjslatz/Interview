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
        private readonly string _docsFolder;
        private readonly string _publicFolder;
        private readonly string _documentCsvFilePath;
        private readonly ICsvHelperExtension _csvHelperExtension;

        public DocumentController(ICsvHelperExtension csvHelperExtension)
        {
            _csvHelperExtension = csvHelperExtension;
            _docsFolder = Path.Combine(Directory.GetCurrentDirectory(), "ClientApp\\public\\Docs");
            _publicFolder = Path.Combine(Directory.GetCurrentDirectory(), "ClientApp\\public");
            _documentCsvFilePath = Path.Combine(_docsFolder, "Documents.csv");
        }

        [HttpGet]
        public ActionResult<List<CsvData>> Get()
        {
            return _csvHelperExtension.ReadCsvFile(_documentCsvFilePath);
        }

        [HttpDelete]
        public ActionResult Delete([FromQuery] string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(path))
                {
                    var filePathToDelete = Path.Combine(_publicFolder, path);
                    if (System.IO.File.Exists(filePathToDelete))
                    {
                        var csvDataList = _csvHelperExtension.ReadCsvFile(_documentCsvFilePath);

                        var csvLines = System.IO.File.ReadAllLines(_documentCsvFilePath).ToList();

                        csvLines.RemoveAll(line => line.Contains(path));

                        System.IO.File.WriteAllLines(_documentCsvFilePath, csvLines);

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
                    var filePath = Path.Combine(_docsFolder, fileName);

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

                    var csvDataList = _csvHelperExtension.ReadCsvFile(_documentCsvFilePath);

                    csvDataList.Add(newCsvEntry);

                    _csvHelperExtension.WriteCsvFile(_documentCsvFilePath, csvDataList);

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
