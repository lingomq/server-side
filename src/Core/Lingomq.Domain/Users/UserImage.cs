using LingoMQ.Core.Domain.Common;

namespace LingoMQ.Core.Domain.Users;

public class UserImage : EntityBase<Guid>
{
    public int X { get; set; }
    public int Y { get; set; }

    public UserImage(int x, int y)
    {
        X = x;
        Y = y;
    }
}
