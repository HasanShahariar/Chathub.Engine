using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using ChatHub.Domain.Entity.setup;
namespace ChatHub.Application.Common.Interfaces;

public interface ITokenService
{
    JwtSecurityToken GenerateJWToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
