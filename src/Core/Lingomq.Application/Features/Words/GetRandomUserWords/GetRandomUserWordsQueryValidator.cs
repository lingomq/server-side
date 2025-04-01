using FluentValidation;

namespace LingoMQ.Core.Application.Features.Words.GetRandomUserWords;

public class GetRandomUserWordsQueryValidator : AbstractValidator<GetRandomUserWordsQuery>
{
    public GetRandomUserWordsQueryValidator()
    {
        RuleFor(x => x.Limit).LessThan(30).WithMessage("Значение не может быть больше 30");
    }
}
