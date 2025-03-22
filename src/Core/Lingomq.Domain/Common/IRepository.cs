using System.Linq.Expressions;

namespace LingoMQ.Core.Domain.Common;

public interface IRepository<T>
{
    Task<T?> FindAsync(
        Expression<Func<T, bool>>? spec = null,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<T>> GetAsync(
        Expression<Func<T, bool>>? spec = null,
        int take = int.MaxValue,
        int skip = 0,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task RemoveAsync(T entity, CancellationToken cancellationToken = default);
}
