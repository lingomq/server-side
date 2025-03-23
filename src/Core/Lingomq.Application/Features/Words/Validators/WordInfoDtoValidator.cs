using FluentValidation;

namespace LingoMQ.Core.Application.Features.Words.Validators;

public class WordInfoDtoValidator : AbstractValidator<WordInfoDto>
{
    public WordInfoDtoValidator()
    {
        RuleFor(x => x.Description).MaximumLength(100);
        RuleFor(x => x.Word).NotNull().NotEmpty().MaximumLength(20);
        RuleFor(x => x.Transcription).MaximumLength(30);
        RuleFor(x => x.Language).SetValidator(new LanguageDtoValidator());
        RuleFor(x => x.Thematics)
            .SetValidator(new WordThematicsDtoValidator()!)
            .When(x => x.Thematics is not null);
    }
}
