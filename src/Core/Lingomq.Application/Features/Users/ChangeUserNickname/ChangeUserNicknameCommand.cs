using LingoMQ.Core.Domain.Users;
using MediatR;

namespace LingoMQ.Core.Application.Features.Users.ChangeUserNickname;

public class ChangeUserNicknameCommand : IRequest
{
    public Guid Id { get; set; }
    public required string Nickname { get; set; }
}

public class ChangeUserNicknameCommandHandler(
    IUsersUnitOfWork unitOfWork,
    IUserRepository userRepository
) : IRequestHandler<ChangeUserNicknameCommand>
{
    public async Task Handle(ChangeUserNicknameCommand request, CancellationToken cancellationToken)
    {
        User user =
            await userRepository.FindAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new InvalidDataException("User wasn't found");

        user.ChangeNickname(request.Nickname);
        await userRepository.UpdateAsync(user, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}
