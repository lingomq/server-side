using System.Linq.Expressions;
using LingoMQ.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace LingoMQ.Infrastructure.Persistense.EntityFramework.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UsersDbContext _context;

    public UserRepository(UsersDbContext context)
    {
        _context = context;
    }

    public async Task<UserImage> SetImageAsync(
        Guid userId,
        UserImage image,
        CancellationToken cancellationToken = default
    )
    {
        User user =
            await _context.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken)
            ?? throw new InvalidDataException("User wasn't found");
        user.SetImage(image);

        return image;
    }

    public async Task AddAsync(User entity, CancellationToken cancellationToken = default) =>
        await _context.Users.AddAsync(entity, cancellationToken);

    public async Task<User?> FindAsync(
        Expression<Func<User, bool>>? spec = null,
        CancellationToken cancellationToken = default
    )
    {
        return spec is not null
            ? await _context.Set<User>().Order().FirstOrDefaultAsync(spec, cancellationToken)
            : await _context.Set<User>().Order().FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAsync(
        Expression<Func<User, bool>>? spec = null,
        int take = int.MaxValue,
        int skip = 0,
        CancellationToken cancellationToken = default
    )
    {
        return spec is not null
            ? await _context
                .Set<User>()
                .Order()
                .Where(spec)
                .Take(take)
                .Skip(skip)
                .ToListAsync(cancellationToken)
            : await _context.Set<User>().Order().Take(take).Skip(skip).ToListAsync();
    }

    public Task RemoveAsync(User entity, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => _context.Set<User>().Remove(entity));
    }

    public Task UpdateAsync(User entity, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => _context.Set<User>().Update(entity));
    }
}
