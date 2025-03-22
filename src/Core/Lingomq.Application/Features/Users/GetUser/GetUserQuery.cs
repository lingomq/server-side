using AutoMapper;
using LingoMQ.Core.Domain.Users;
using MediatR;

namespace LingoMQ.Core.Application.Features.Users.GetUser;

public class GetUserQuery : IRequest<UserDto>
{
    public Guid UserId { get; set; }
}

public class GetUserQueryHandler(IMapper mapper, IUserRepository userRepository)
    : IRequestHandler<GetUserQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        User user =
            await userRepository.FindAsync(x => x.Id == request.UserId, cancellationToken)
            ?? throw new InvalidDataException("User wasn't found");
        return mapper.Map<UserDto>(user);
    }
}
