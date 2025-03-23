using FluentValidation;

namespace LingoMQ.Core.Application.Features.Words.GetWords;

public class GetWordsQueryValidator : AbstractValidator<GetWordsQuery>
{
    public GetWordsQueryValidator()
    {
        RuleFor(x => x.Language).NotNull();
    }
}
