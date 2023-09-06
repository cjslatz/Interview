using DocumentManager.Models;

namespace DocumentManager.Controllers
{
    public interface ICsvHelperExtension
    {
        List<CsvData> ReadCsvFile(string filePath);
        void WriteCsvFile(string filePath, List<CsvData> csvDataList);
    }
}
