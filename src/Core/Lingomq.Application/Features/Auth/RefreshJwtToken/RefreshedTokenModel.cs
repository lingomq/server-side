namespace LingoMQ.Core.Application.Features.Auth.RefreshJwtToken;

public class RefreshedTokenModel
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime RefreshExpiresAt { get; set; }
}
