using LingoMQ.Core.Domain.Users;
using MediatR;

namespace LingoMQ.Core.Application.Features.Users.GetUserEmail;

public class GetUserEmailQuery : IRequest<GetUserEmailResponse>
{
    public Guid Id { get; set; }
}

public class GetUserEmailQueryHandler : IRequestHandler<GetUserEmailQuery, GetUserEmailResponse>
{
    private readonly IUserRepository _userRepository;

    public GetUserEmailQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<GetUserEmailResponse> Handle(
        GetUserEmailQuery request,
        CancellationToken cancellationToken
    )
    {
        User user =
            await _userRepository.FindAsync(
                x => x.Id == request.Id,
                cancellationToken: cancellationToken
            ) ?? throw new InvalidDataException("User wasn't found");

        var emailType =
            user.Credentials.AuthorizationTypes.FirstOrDefault(x =>
                x.Type == AuthorizationTypeEnum.Email
            ) ?? throw new InvalidDataException("User email wasn't found");

        return new() { Email = emailType.Value };
    }
}
