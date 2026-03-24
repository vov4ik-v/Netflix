using Netflix.DataAccess.Interfaces;

namespace Netflix.DataAccess.CsvReading;

public class CsvDataReader : ICsvDataReader
{
    public async Task<List<string[]>> ReadAllRowsAsync(string filePath)
    {
        var rows = new List<string[]>();
        var lines = await File.ReadAllLinesAsync(filePath);

        for (var i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            var fields = ParseCsvLine(lines[i]);
            rows.Add(fields);
        }

        return rows;
    }

    public string[] GetHeaders(string filePath)
    {
        using var reader = new StreamReader(filePath);
        var headerLine = reader.ReadLine();
        if (headerLine == null)
            return Array.Empty<string>();

        return ParseCsvLine(headerLine);
    }

    private static string[] ParseCsvLine(string line)
    {
        var fields = new List<string>();
        var current = string.Empty;
        var inQuotes = false;

        for (var i = 0; i < line.Length; i++)
            if (inQuotes)
            {
                if (line[i] == '"')
                {
                    if (i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current += '"';
                        i++;
                    }
                    else
                    {
                        inQuotes = false;
                    }
                }
                else
                {
                    current += line[i];
                }
            }
            else
            {
                if (line[i] == '"')
                {
                    inQuotes = true;
                }
                else if (line[i] == ',')
                {
                    fields.Add(current);
                    current = string.Empty;
                }
                else
                {
                    current += line[i];
                }
            }

        fields.Add(current);
        return fields.ToArray();
    }
}