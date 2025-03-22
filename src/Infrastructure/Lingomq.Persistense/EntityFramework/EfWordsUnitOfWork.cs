using LingoMQ.Core.Application.Features.Words;

namespace LingoMQ.Infrastructure.Persistense.EntityFramework;

public class EfWordsUnitOfWork(WordsDbContext context) : IWordsUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken = default) =>
        await context.SaveChangesAsync(cancellationToken);

    public Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
