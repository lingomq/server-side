using System.Linq.Expressions;

namespace LingoMQ.Core.Domain.Common.Specifications;

public class NotSpecification<T> : Specification<T>
{
    private Specification<T> _specification;

    public NotSpecification(Specification<T> specification) => _specification = specification;

    public override Expression<Func<T, bool>> ToExpression()
    {
        return Expression.Lambda<Func<T, bool>>(
            Expression.Not(_specification.ToExpression().Body),
            _specification.ToExpression().Parameters.Single()
        );
    }
}
