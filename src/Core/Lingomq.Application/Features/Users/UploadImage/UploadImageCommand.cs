using LingoMQ.Core.Application.Common;
using LingoMQ.Core.Domain.Users;
using MediatR;

namespace LingoMQ.Core.Application.Features.Users.UploadImage;

public class UploadImageCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public UploadImageCommand(Guid userId, int x, int y)
    {
        UserId = userId;
        X = x;
        Y = y;
    }
}

public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, Guid>
{
    private readonly IUsersUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public UploadImageCommandHandler(IUsersUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(UploadImageCommand request, CancellationToken cancellationToken)
    {
        var image = await _userRepository.SetImageAsync(request.UserId, new(request.X, request.Y));
        await _unitOfWork.CommitAsync(cancellationToken);

        return image.Id;
    }
}
