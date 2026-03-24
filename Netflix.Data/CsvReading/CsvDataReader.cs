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
        return headerLine == null ? [] : ParseCsvLine(headerLine);
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
                switch (line[i])
                {
                    case '"':
                        inQuotes = true;
                        break;
                    case ',':
                        fields.Add(current);
                        current = string.Empty;
                        break;
                    default:
                        current += line[i];
                        break;
                }
            }

        fields.Add(current);
        return fields.ToArray();
    }
}