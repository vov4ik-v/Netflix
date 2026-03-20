namespace Netflix.DAL.Interfaces;

public interface ICsvDataReader
{
    Task<List<string[]>> ReadAllRowsAsync(string filePath);
    string[] GetHeaders(string filePath);
}