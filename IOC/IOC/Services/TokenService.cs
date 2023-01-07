using IOC.DataBase;
using IOC.Models;
using IOC.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IOC.Services
{
    public class TokenService : ITokenService
    {
        private readonly DatabaseContext _context;
        private JWTSettings _config;

        public TokenService(DatabaseContext context, IOptions<JWTSettings> config)
        {
            _context = context;
            _config = config.Value;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Key));
            var loginCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: _config.Issuer,
                audience: _config.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(2),
                signingCredentials: loginCredentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;
        }

        public string GenerateRefreshToken()
        {
            var rand = new byte[32];
            using (var randGen = RandomNumberGenerator.Create())
            {
                randGen.GetBytes(rand);
                return Convert.ToBase64String(rand);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string expiredToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _config.Issuer,
                ValidAudience = _config.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Key)),
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(expiredToken, tokenValidationParameters,
                out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        public async Task<AuthenticatedResponse> RefreshToken(TokenAPI tokenAPI)
        {
            string accessToken = tokenAPI.AccessToken;
            string refreshToken = tokenAPI.RefreshToken;
            var principal = GetPrincipalFromExpiredToken(accessToken);
            var email = principal.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.MailAddress == email);
            var refreshTokenUser = await _context.UserRefreshTokens.FirstOrDefaultAsync(r => r.IDUser == user.IDUser);
            if (user is null || refreshTokenUser.RefreshToken != refreshToken
                || refreshTokenUser.RefreshTokenExpiryTime <= DateTime.Now)
                return null;

            var newAccessToken = GenerateAccessToken(principal.Claims);
            var newRefreshToken = GenerateRefreshToken();
            var refreshExpiryDate = DateTime.Now.AddDays(7);
            refreshTokenUser.RefreshToken = newRefreshToken;
            await _context.SaveChangesAsync();

            return new AuthenticatedResponse()
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task<bool> Revoke(string email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.MailAddress == email);
            var refreshTokenUser = await _context.UserRefreshTokens.FirstOrDefaultAsync(r => r.IDUser == user.IDUser);
            if (refreshTokenUser == null)
                return false;

            refreshTokenUser.RefreshToken = null;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
