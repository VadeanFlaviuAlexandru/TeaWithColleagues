using IOC.Models;
using System.Security.Claims;

namespace IOC.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string expiredToken);
        public Task<AuthenticatedResponse> RefreshToken(TokenAPI tokenAPI);
        public Task<bool> Revoke(string email);
    }
}
