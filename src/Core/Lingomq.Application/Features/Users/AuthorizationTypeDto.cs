using LingoMQ.Core.Domain.Users;

namespace LingoMQ.Core.Application.Features.Users;

public class AuthorizationTypeDto
{
    public AuthorizationTypeEnum Type { get; set; }
    public string Value { get; set; } = "";
}
