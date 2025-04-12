namespace LingoMQ.Core.Application.Features.Auth.RefreshJwtToken;

public class RefreshJwtTokenRequest
{
    public required string RefreshToken { get; set; }
}
