using FluentValidation;

namespace LingoMQ.Core.Application.Words.Validations;

public class WordThematicsDtoValidator : AbstractValidator<WordThematicsDto>
{
    public WordThematicsDtoValidator()
    {
        RuleFor(x => x.Category).NotNull().MinimumLength(3).MaximumLength(15);
    }
}
