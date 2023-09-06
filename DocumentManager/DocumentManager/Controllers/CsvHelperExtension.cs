using CsvHelper;
using CsvHelper.Configuration;
using DocumentManager.Models;
using System.Globalization;

namespace DocumentManager.Controllers
{
    public class CsvHelperExtensions : ICsvHelperExtension
    {
        public List<CsvData> ReadCsvFile(string filePath)
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

        public void WriteCsvFile(string filePath, List<CsvData> csvDataList)
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
    }
}
