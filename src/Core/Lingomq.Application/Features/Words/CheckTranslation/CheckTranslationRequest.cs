namespace LingoMQ.Core.Application.Features.Words.CheckTranslation;

public class CheckTranslationRequest
{
    public Guid WordId { get; set; }
    public required string Translation { get; set; }
    public required LanguageDto LanguageTo { get; set; }
}
