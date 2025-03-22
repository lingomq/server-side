namespace LingoMQ.Core.Application.Features.Users;

public class UserDto
{
    public Guid Id { get; set; }
    public required string Nickname { get; set; }
    public string? Description { get; set; }
    public UserImageDto Image { get; set; } = new() { X = 0, Y = 0 };
    public UserRoleDto? Role { get; set; }
}