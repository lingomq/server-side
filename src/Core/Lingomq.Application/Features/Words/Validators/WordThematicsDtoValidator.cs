using FluentValidation;

namespace LingoMQ.Core.Application.Features.Words.Validators;

public class WordThematicsDtoValidator : AbstractValidator<WordThematicsDto>
{
    public WordThematicsDtoValidator()
    {
        RuleFor(x => x.Category).NotNull().MinimumLength(3).MaximumLength(15);
    }
}
