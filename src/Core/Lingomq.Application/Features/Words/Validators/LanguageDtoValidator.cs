using FluentValidation;

namespace LingoMQ.Core.Application.Features.Words.Validators;

public class LanguageDtoValidator : AbstractValidator<LanguageDto>
{
    public LanguageDtoValidator()
    {
        RuleFor(x => x.Value).NotNull().MaximumLength(15);
        RuleFor(x => x.Code).NotNull().MaximumLength(5);
        RuleFor(x => x.SubCode).NotNull().MaximumLength(5);
    }
}
