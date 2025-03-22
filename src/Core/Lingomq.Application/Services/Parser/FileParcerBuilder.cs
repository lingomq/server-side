namespace LingoMQ.Core.Application.Services.Parser;

public static class FileParcerBuilder
{
    public static IFileParser BuildInstanse(string fileExtension)
    {
        switch (fileExtension)
        {
            case "csv":
                return new CsvFileParser();
            default:
                throw new InvalidDataException("Invalid file extension was sended");
        }
    }
}
