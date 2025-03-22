using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LingoMQ.Core.Application.Features.Auth.Jwt;
using LingoMQ.Core.Domain.Users;
using MediatR;

namespace LingoMQ.Core.Application.Features.Auth.SignIn;

public class SignInCommand : IRequest<AuthTokenModel>
{
    public required SignInModel SignModel { get; set; }
}

public class SignInCommandHandler(IJwtService jwtService, IUserRepository userRepository)
    : IRequestHandler<SignInCommand, AuthTokenModel>
{
    public async Task<AuthTokenModel> Handle(
        SignInCommand request,
        CancellationToken cancellationToken
    )
    {
        AuthorizationType authorizationType = request.SignModel.Type switch
        {
            AuthorizationTypeEnum.Email => AuthorizationType.AsEmail(request.SignModel.SignKey),
            AuthorizationTypeEnum.Phone => AuthorizationType.AsPhone(request.SignModel.SignKey),
            _ => throw new ApplicationException("Invalid authorization type parameter"),
        };

        User user =
            await userRepository.FindAsync(x =>
                x.Credentials.AuthorizationTypes.Any(x =>
                    x.Type == authorizationType.Type && x.Value == authorizationType.Value
                )
            ) ?? throw new InvalidDataException("Invalid credentials");

        var isValid = user.Credentials.CheckValidity(
            authorizationType,
            request.SignModel.SignValue
        );
        if (!isValid)
            throw new InvalidDataException("Invalid credentials");
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
        JwtSecurityToken accessToken = jwtService.CreateToken(
            accessTokenClaims,
            DateTime.Now.AddMinutes(jwtService.Configuration.AccessTokenExpires)
        );
        JwtSecurityToken refreshToken = jwtService.CreateToken(
            refreshTokenClaims,
            DateTime.Now.AddMinutes(jwtService.Configuration.RefreshTokenExpires)
        );

        return new AuthTokenModel()
        {
            AccessToken = jwtService.WriteToken(accessToken),
            RefreshToken = jwtService.WriteToken(refreshToken),
            ExpiresAt = DateTime.Now.AddMinutes(jwtService.Configuration.AccessTokenExpires),
            RefreshExpiresAt = DateTime.Now.AddMinutes(
                jwtService.Configuration.RefreshTokenExpires
            ),
        };
    }
}
