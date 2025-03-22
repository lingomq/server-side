using LingoMQ.Core.Domain.Users;

namespace LingoMQ.Core.Application.Features.Users.CreateUser;

public class SignUpModel 
{
    public AuthorizationTypeEnum Type { get; set; }
    public required string SignKey { get; set; }
    public required string SignValue { get; set; }
}