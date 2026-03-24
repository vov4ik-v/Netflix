namespace Netflix.BusinessLogic.Interfaces;

public interface IDataImportService
{
    Task ImportFromCsvAsync(string filePath);
}