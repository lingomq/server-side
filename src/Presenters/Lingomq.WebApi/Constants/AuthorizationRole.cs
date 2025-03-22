namespace LingoMQ.Presenters.WebApi.Constants;

public class AuthorizationRoles
{
    public const string Users = "user";
    public const string Moderations = "moderator";
    public const string Admin = "admin";
    public const string Everyone = "user,moderator,admin";
    public const string Staff = "moderator,admin";
}
