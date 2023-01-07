using IOC.Models;
using IOC.RequestModels;

namespace IOC.Services.Interfaces
{
    public interface ILoginService
    {
        public Task<Object> CreateTokenString(LoginRequest user);
        public bool isPasswordValid(LoginRequest user, User userByEmail);
        public Task EditTokenTable(string refreshToken, UserRefreshToken userRefreshToken);
        public Task AddTokenTable(User user, string refreshToken);
        public Task<User> FindUserByEmail(string email);
        public Task<UserRefreshToken> FindRefreshTokenByIDUser(User user);
        //public Task<bool> ResetPassword(ResetPassword reset);
        //public Task<bool> ForgotPassword(string email, string encryptedEmail);

    }
}
