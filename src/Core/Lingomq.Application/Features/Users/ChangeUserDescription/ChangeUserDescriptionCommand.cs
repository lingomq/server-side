using LingoMQ.Core.Domain.Users;
using MediatR;

namespace LingoMQ.Core.Application.Features.Users.ChangeUserDescription;

public class ChangeUserDescriptionCommand : IRequest
{
    public Guid Id { get; set; }
    public required string Description { get; set; }
}

public class ChangeUserDescriptionCommandHandler : IRequestHandler<ChangeUserDescriptionCommand>
{
    private readonly IUsersUnitOfWork _usersUnitOfWork;
    private readonly IUserRepository _userRepository;

    public ChangeUserDescriptionCommandHandler(
        IUsersUnitOfWork usersUnitOfWork,
        IUserRepository userRepository
    )
    {
        _usersUnitOfWork = usersUnitOfWork;
        _userRepository = userRepository;
    }

    public async Task Handle(
        ChangeUserDescriptionCommand request,
        CancellationToken cancellationToken
    )
    {
        User user =
            await _userRepository.FindAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new InvalidDataException("User wasn't found");

        user.ChangeDescription(request.Description);
        await _userRepository.UpdateAsync(user, cancellationToken);
        await _usersUnitOfWork.CommitAsync(cancellationToken);
    }
}
