using LingoMQ.Core.Domain.Users;
using MediatR;

namespace LingoMQ.Core.Application.Features.Users.GetUserImage;

public class GetUserImageQuery : IRequest<string>
{
    public Guid UserId { get; set; }
}

public class GetUserImageQueryHandler : IRequestHandler<GetUserImageQuery, string>
{
    private readonly IUserRepository _userRepository;

    public GetUserImageQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<string> Handle(
        GetUserImageQuery request,
        CancellationToken cancellationToken
    )
    {
        User user =
            await _userRepository.FindAsync(
                x => x.Id == request.UserId,
                cancellationToken: cancellationToken
            ) ?? throw new InvalidDataException("User wasn't found");

        if (!Directory.Exists("Binaries/Images"))
            Directory.CreateDirectory("Binaries/Images");
        if (!Directory.Exists("Binaries/Images/Users"))
            Directory.CreateDirectory("Binaries/Images/Users");

        if (user.Image is null)
            return Directory.GetCurrentDirectory() + "/Binaries/Images/default.png";

        return Directory.GetCurrentDirectory()
            + "/Binaries/Images/Users/"
            + user.Image.Id
            + ".png";
    }
}
