namespace LingoMQ.Core.Application.Features.Users.ChangeUserEmail;

public class ChangeUserEmailModel
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
}
