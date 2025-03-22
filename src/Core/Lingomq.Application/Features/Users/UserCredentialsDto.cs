namespace LingoMQ.Core.Application.Features.Users;

public class UserCredentialsDto
{
    public int Id { get; set; }
    public List<AuthorizationTypeDto> AuthorizationTypes { get; set; } = new();
    public string PasswordHash { get; set; } = "";
    public string PasswordSalt { get; set; } = "";
}
