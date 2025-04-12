using LingoMQ.Core.Domain.Common;

namespace LingoMQ.Core.Domain.Users;

public class User : EntityBase<Guid>
{
    public virtual UserCredentials Credentials { get; private set; }
    public string Nickname { get; private set; }
    public string Description { get; private set; }
    public UserRole Role { get; private set; }
    public virtual UserImage? Image { get; private set; }

    public User(
        string nickname,
        AuthorizationType authorizationType,
        string password,
        string description = ""
    )
    {
        Nickname = nickname;
        Role = UserRole.User;
        Credentials = new(authorizationType, password);
        Description = description;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    protected User() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public void ChangeNickname(string nickname) => Nickname = nickname;

    public void ChangeDescription(string description) => Description = description;

    public void ChangeRole(User changer, UserRole role)
    {
        if (changer.Role.Weight < role.Weight)
            throw new InvalidDataException();

        Role = role;
    }

    public void SetImage(UserImage image) => Image = image;
}
