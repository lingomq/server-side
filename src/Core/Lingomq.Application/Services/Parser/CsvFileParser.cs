using System.Globalization;
using CsvHelper;

namespace LingoMQ.Core.Application.Services.Parser;

public class CsvFileParser : IFileParser
{
    public IEnumerable<T> ParseData<T>(
        string filePath,
        CancellationToken cancellationToken = default
    )
    {
        IEnumerable<T> records = new List<T>();
        using (TextReader reader = new StreamReader(filePath))
        {
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                records = csv.GetRecords<T>().ToList();
            }
        }

        return records;
    }
}
