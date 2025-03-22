using LingoMQ.Core.Domain.Common;

namespace LingoMQ.Core.Domain.Users;

public class UserRole : ValueObject
{
    private int _weight;
    private string _name;

    public int Weight => _weight;
    public string Name => _name;

    public static UserRole User => new("user", 1 << 0);
    public static UserRole Moderator => new("moderator", 1 << 1);
    public static UserRole Admin => new("admin", 1 << 2);

    protected UserRole(string name, int weight)
    {
        _name = name;
        _weight = weight;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Weight;
    }
}
