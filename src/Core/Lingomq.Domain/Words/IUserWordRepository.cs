using LingoMQ.Core.Domain.Common;

namespace LingoMQ.Core.Domain.Words;

public interface IUserWordRepository : IRepository<UserWord>
{
    Task<IEnumerable<UserWord>> GetRandomUserWordsAsync(
        Guid userId,
        int limit,
        CancellationToken cancellationToken = default
    );
}
