namespace LingoMQ.Core.Application.Features.Words;

public class WordInfoDto
{
    public Guid Id { get; set; }
    public required string Word { get; set; }
    public string Transcription { get; set; } = "";
    public string Description { get; set; } = "";
    public List<WordInfoDto>? Translations { get; set; } = new();
    public required LanguageDto Language { get; set; }
    public WordThematicsDto? Thematics { get; set; }
}
