using LingoMQ.Core.Domain.Users;
using LingoMQ.Core.Domain.Words;

namespace LingoMQ.Infrastructure.Persistense.EntityFramework.Repositories.Words.Models;

public class UserWordDao
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid WordId { get; set; }

    public virtual User? User { get; set; }
    public virtual WordInfo? Word { get; set; }
}
