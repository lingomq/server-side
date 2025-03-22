using AutoMapper;
using LingoMQ.Core.Application.Common;
using LingoMQ.Core.Domain.Users;
using MediatR;

namespace LingoMQ.Core.Application.Features.Users.ChangeUserRole;

public class ChangeUserRoleCommand : IRequest
{
    public required UserRoleDto Role { get; set; }
    public Guid ChangerId { get; set; }
    public Guid UserId { get; set; }
}

public class ChangeUserRoleCommandHandler(
    IUsersUnitOfWork unitOfWork,
    IMapper mapper,
    IUserRepository userRepository
) : IRequestHandler<ChangeUserRoleCommand>
{
    public async Task Handle(ChangeUserRoleCommand request, CancellationToken cancellationToken)
    {
        var userRole = mapper.Map<UserRole>(request.Role);
        var changer =
            await userRepository.FindAsync(x => x.Id == request.ChangerId, cancellationToken)
            ?? throw new InvalidDataException("Changer user wasn't found");
        var user =
            await userRepository.FindAsync(x => x.Id == request.UserId, cancellationToken)
            ?? throw new InvalidDataException("User wasn't found");
        user.ChangeRole(changer, userRole);
        await userRepository.UpdateAsync(user, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}
