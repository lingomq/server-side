using LingoMQ.Core.Domain.Common;

namespace LingoMQ.Core.Domain.Users;

public enum AuthorizationTypeEnum
{
    Email,
    Phone,
    OAuth,
}

public class AuthorizationType : ValueObject
{
    public int Id { get; private set; }
    private AuthorizationTypeEnum _type;
    private string _value;
    public AuthorizationTypeEnum Type => _type;
    public string Value => _value;

    public static AuthorizationType AsEmail(string value) =>
        new(AuthorizationTypeEnum.Email, value);

    public static AuthorizationType AsPhone(string value) =>
        new(AuthorizationTypeEnum.Phone, value);

    protected AuthorizationType(AuthorizationTypeEnum type, string value)
    {
        _type = type;
        _value = value;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    protected AuthorizationType() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Type;
        yield return Value;
    }
}

public static class AuthorizationTypeEnumResolver
{
    public static string Resolve(AuthorizationTypeEnum typeEnum) =>
        typeEnum switch
        {
            AuthorizationTypeEnum.Email => "email",
            AuthorizationTypeEnum.Phone => "phone",
            AuthorizationTypeEnum.OAuth => "oauth",
            _ => throw new InvalidDataException("AuthorizationType wasn't found"),
        };
}
