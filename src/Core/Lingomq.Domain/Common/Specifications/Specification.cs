using System.Linq.Expressions;

namespace LingoMQ.Core.Domain.Common.Specifications;

public abstract class Specification<T>
{
    public abstract Expression<Func<T, bool>> ToExpression();

    public Specification<T> And(Specification<T> specification) =>
        new AndSpecification<T>(this, specification);

    public Specification<T> Or(Specification<T> specification) =>
        new OrSpecification<T>(this, specification);

    public Specification<T> Not() => new NotSpecification<T>(this);

    public bool IsSatisfiedBy(T item)
    {
        Func<T, bool> predicate = ToExpression().Compile();
        return predicate(item);
    }
}
