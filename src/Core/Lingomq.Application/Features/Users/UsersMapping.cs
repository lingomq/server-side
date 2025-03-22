using AutoMapper;
using LingoMQ.Core.Application.Features.Users;
using LingoMQ.Core.Domain.Users;

namespace LingoMQ.Core.Application.Features;

public class UsersProfileMapping : Profile
{
    public UsersProfileMapping()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<UserCredentials, UserCredentialsDto>().ReverseMap();
        CreateMap<UserRole, UserRoleDto>().ReverseMap();
        CreateMap<AuthorizationType, AuthorizationTypeDto>().ReverseMap();
        CreateMap<UserImage, UserImageDto>().ReverseMap();
    }
}
