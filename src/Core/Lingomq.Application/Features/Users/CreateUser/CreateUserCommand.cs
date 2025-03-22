using LingoMQ.Core.Application.Common;
using LingoMQ.Core.Domain.Users;
using LingoMQ.Core.Domain.Users.Events;
using MediatR;

namespace LingoMQ.Core.Application.Features.Users.CreateUser;

public class CreateUserCommand : IRequest
{
    public required UserDto UserDto { get; set; }
    public required SignUpModel SignModel { get; set; }
}

public class CreateUserCommandHandler(
    IUsersUnitOfWork unitOfWork,
    IUserRepository userRepository,
    IMediator mediator
) : IRequestHandler<CreateUserCommand>
{
    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        AuthorizationType authorizationType = request.SignModel.Type switch
        {
            AuthorizationTypeEnum.Email => AuthorizationType.AsEmail(request.SignModel.SignKey),
            AuthorizationTypeEnum.Phone => AuthorizationType.AsPhone(request.SignModel.SignKey),
            _ => throw new ApplicationException("Invalid authorization type parameter"),
        };

        User user = new User(
            request.UserDto.Nickname,
            authorizationType,
            request.SignModel.SignValue,
            request.UserDto.Description ?? ""
        );

        user.AddEvent(new UserCreatedEvent(user, authorizationType));
        foreach (var e in user.DomainEvents)
            await mediator.Publish(e, cancellationToken);

        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}
