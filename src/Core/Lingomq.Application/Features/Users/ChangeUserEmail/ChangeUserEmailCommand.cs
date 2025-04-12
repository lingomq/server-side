using LingoMQ.Core.Domain.Users;
using MediatR;

namespace LingoMQ.Core.Application.Features.Users.ChangeUserEmail;

public class ChangeUserEmailCommand : IRequest
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
}

public class ChangeUserEmailCommandHandler : IRequestHandler<ChangeUserEmailCommand>
{
    private readonly IUsersUnitOfWork _usersUnitOfWork;
    private readonly IUserRepository _userRepository;

    public ChangeUserEmailCommandHandler(
        IUsersUnitOfWork usersUnitOfWork,
        IUserRepository userRepository
    )
    {
        _usersUnitOfWork = usersUnitOfWork;
        _userRepository = userRepository;
    }

    public async Task Handle(ChangeUserEmailCommand request, CancellationToken cancellationToken)
    {
        User user =
            await _userRepository.FindAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new InvalidDataException("User wasn't found");

        var userEmail =
            user.Credentials.AuthorizationTypes.FirstOrDefault(x =>
                x.Type == AuthorizationTypeEnum.Email
            ) ?? throw new InvalidDataException("Email by this user wasn't found");
            
        user.Credentials.ChangeAuthorizationTypeValue(
            AuthorizationTypeEnum.Email,
            userEmail.Value,
            request.Email
        );

        await _userRepository.UpdateAsync(user, cancellationToken);
        await _usersUnitOfWork.CommitAsync(cancellationToken);
    }
}
