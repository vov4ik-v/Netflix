namespace Netflix.BLL.Interfaces;

public interface IDataImportService
{
    Task ImportFromCsvAsync(string filePath);
}