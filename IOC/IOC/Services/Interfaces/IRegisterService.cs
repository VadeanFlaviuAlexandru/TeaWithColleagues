using IOC.Models;

namespace IOC.Services.Interfaces
{
    public interface IRegisterService
    {
        public Task<bool> CheckIfUserExists(User user);
        public Task<bool> AddUserToDatabase(User user);
    }
}
