using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LingoMQ.Core.Application.Features.Auth.Jwt;
using LingoMQ.Core.Domain.Users;
using MediatR;

namespace LingoMQ.Core.Application.Features.Auth.RefreshJwtToken;

public class RefreshJwtTokenCommand : IRequest<RefreshedTokenModel>
{
    public RefreshJwtTokenRequest Request { get; set; }

    public RefreshJwtTokenCommand(RefreshJwtTokenRequest request)
    {
        Request = request;
    }
}

public class RefreshJwtTokenCommandHandler
    : IRequestHandler<RefreshJwtTokenCommand, RefreshedTokenModel>
{
    private readonly IJwtService _jwtService;
    private readonly IUserRepository _userRepository;

    public RefreshJwtTokenCommandHandler(IUserRepository userRepository, IJwtService jwtService)
    {
        _jwtService = jwtService;
        _userRepository = userRepository;
    }

    public async Task<RefreshedTokenModel> Handle(
        RefreshJwtTokenCommand request,
        CancellationToken cancellationToken
    )
    {
        var credentials =
            _jwtService.GetClaimsPrincipal(request.Request.RefreshToken)
            ?? throw new InvalidDataException("InvalidToken");

        var userId = credentials.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

        var user =
            await _userRepository.FindAsync(x => x.Id == Guid.Parse(userId), cancellationToken)
            ?? throw new InvalidDataException("User wasn't found");

        List<Claim> accessTokenClaims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.Name),
            new Claim(ClaimTypes.Version, "access"),
        };

        List<Claim> refreshTokenClaims = new()
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role!.Name!),
            new Claim(ClaimTypes.Version, "refresh"),
        };
        JwtSecurityToken accessToken = _jwtService.CreateToken(
            accessTokenClaims,
            DateTime.Now.AddMinutes(_jwtService.Configuration.AccessTokenExpires)
        );
        JwtSecurityToken refreshToken = _jwtService.CreateToken(
            refreshTokenClaims,
            DateTime.Now.AddMinutes(_jwtService.Configuration.RefreshTokenExpires)
        );

        return new RefreshedTokenModel()
        {
            AccessToken = _jwtService.WriteToken(accessToken),
            RefreshToken = _jwtService.WriteToken(refreshToken),
            ExpiresAt = DateTime.Now.AddMinutes(_jwtService.Configuration.AccessTokenExpires),
            RefreshExpiresAt = DateTime.Now.AddMinutes(
                _jwtService.Configuration.RefreshTokenExpires
            ),
        };
    }
}
