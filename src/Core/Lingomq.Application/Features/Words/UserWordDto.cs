using LingoMQ.Core.Application.Features.Users;

namespace LingoMQ.Core.Application.Features.Words;

public class UserWordDto
{
    public Guid Id { get; set; }
    public required UserDto User { get; set; }
    public required WordInfoDto Word { get; set; }
}
