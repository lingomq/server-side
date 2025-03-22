using LingoMQ.Core.Application.Common;
using LingoMQ.Core.Domain.Users;
using MediatR;

namespace LingoMQ.Core.Application.Features.Users.ChangeUserPassword;

public class ChangeUserPasswordCommand : IRequest
{
    public Guid UserId { get; set; }
    public required string Password { get; set; }
}

public class ChangeUserPasswordCommandHandler(
    IUsersUnitOfWork unitOfWork,
    IUserRepository userRepository
) : IRequestHandler<ChangeUserPasswordCommand>
{
    public async Task Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
    {
        User user =
            await userRepository.FindAsync(x => x.Id == request.UserId, cancellationToken)
            ?? throw new InvalidDataException("User wasn't found");
        user.Credentials.ChangePassword(request.Password);

        await userRepository.UpdateAsync(user, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}
