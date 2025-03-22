using FluentValidation;

namespace LingoMQ.Core.Application.Words.Queries.QueriesValidators;

public class GetWordsQueryValidator : AbstractValidator<GetWordsQuery>
{
    public GetWordsQueryValidator()
    {
        RuleFor(x => x.Language).NotNull();
    }
}
