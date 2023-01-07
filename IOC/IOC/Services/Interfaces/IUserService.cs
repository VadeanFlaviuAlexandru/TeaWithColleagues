using IOC.Models;

namespace IOC.Services.Interfaces
{
    public interface IUserService
    {
        public Task<User> GetUserByID(int id);
        public Task<User> GetUserByName(string name);
        public Task<List<User>> GetAllUsers();
        public Task<User> CheckIfUserExists(int? id);
        public Task<User> EditUser(User user);
        //public Task<bool> ChangeUserPassword(int id);
    }
}
