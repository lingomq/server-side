using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LingoMQ.Core.Application.Features.Auth.Jwt;

public interface IJwtService
{
    JwtConfiguration Configuration { get; }
    JwtSecurityToken CreateToken(List<Claim> claims, DateTime expires);
    ClaimsPrincipal GetClaimsPrincipal(string token);
    string WriteToken(JwtSecurityToken token);
}
