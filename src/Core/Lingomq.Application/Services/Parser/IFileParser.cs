namespace LingoMQ.Core.Application.Services.Parser;

public interface IFileParser
{
    IEnumerable<T> ParseData<T>(
        string filePath,
        CancellationToken cancellationToken = default
    );
}
