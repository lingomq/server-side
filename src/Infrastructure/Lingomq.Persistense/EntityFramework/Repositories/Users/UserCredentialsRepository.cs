using System.Linq.Expressions;
using LingoMQ.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace LingoMQ.Infrastructure.Persistense.EntityFramework.Repositories.Users;

public class UserCredentialsRepository : IUserCredentialsRepository
{
    private readonly UsersDbContext _context;

    public UserCredentialsRepository(UsersDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        UserCredentials entity,
        CancellationToken cancellationToken = default
    ) => await _context.Set<UserCredentials>().AddAsync(entity, cancellationToken);

    public async Task<UserCredentials?> FindAsync(
        Expression<Func<UserCredentials, bool>>? spec = null,
        CancellationToken cancellationToken = default
    )
    {
        return spec is not null
            ? await _context
                .Set<UserCredentials>()
                .Order()
                .FirstOrDefaultAsync(spec, cancellationToken)
            : await _context.Set<UserCredentials>().Order().FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<UserCredentials>> GetAsync(
        Expression<Func<UserCredentials, bool>>? spec = null,
        int take = int.MaxValue,
        int skip = 0,
        CancellationToken cancellationToken = default
    )
    {
        return spec is not null
            ? await _context
                .Set<UserCredentials>()
                .Order()
                .Where(spec)
                .Take(take)
                .Skip(skip)
                .ToListAsync(cancellationToken)
            : await _context.Set<UserCredentials>().Order().Take(take).Skip(skip).ToListAsync();
    }

    public Task RemoveAsync(UserCredentials entity, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => _context.Set<UserCredentials>().Remove(entity));
    }

    public Task UpdateAsync(UserCredentials entity, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => _context.Set<UserCredentials>().Update(entity));
    }
}
