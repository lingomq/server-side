using LingoMQ.Core.Application.Features.Users;

namespace LingoMQ.Infrastructure.Persistense.EntityFramework;

public class EfUsersUnitOfWork(UsersDbContext context) : IUsersUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken = default) =>
        await context.SaveChangesAsync(cancellationToken);

    public Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
