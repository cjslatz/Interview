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
        public static string docsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Docs");
        public static string docsParent = Directory.GetParent(docsFolder).ToString();
        public static string documentCsvFilePath = Path.Combine(docsFolder, "Documents.csv");

        [HttpGet]
        public ActionResult<List<CsvData>> Get()
        {
            List<CsvData> csvDataList;
            using (var reader = new StreamReader(documentCsvFilePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csvDataList = csv.GetRecords<CsvData>().ToList();
            }
            //foreach(var csvData in csvDataList)
            //{
            //    csvData.Path = Path.Combine(docsParent, csvData.Path);
            //}
            Console.WriteLine(csvDataList.ToString());
            return csvDataList;
        }
    }
}
