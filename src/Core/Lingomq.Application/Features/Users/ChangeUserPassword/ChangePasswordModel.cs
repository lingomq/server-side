namespace LingoMQ.Core.Application.Features.Users.ChangeUserPassword;

public class ChangePasswordModel
{
    public required string OldPassword { get; set; }
    public required string Password { get; set; }
}
