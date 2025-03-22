using LingoMQ.Core.Domain.Common;

namespace LingoMQ.Core.Domain.Users;

public interface IUserRepository : IRepository<User>
{
    Task<UserImage> SetImageAsync(
        Guid userId,
        UserImage image,
        CancellationToken cancellationToken = default
    );
}
