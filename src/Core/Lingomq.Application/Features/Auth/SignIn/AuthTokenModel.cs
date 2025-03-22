namespace LingoMQ.Core.Application.Features.Auth.SignIn;

public class AuthTokenModel
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime RefreshExpiresAt { get; set; }
}