using LingoMQ.Core.Domain.Users;

namespace LingoMQ.Core.Application.Features.Auth.SignIn;

public class SignInModel 
{
    public AuthorizationTypeEnum Type { get; set; }
    public required string SignKey { get; set; }
    public required string SignValue { get; set; }
}