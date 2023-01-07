using IOC.DataBase;
using IOC.Models;
using IOC.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IOC.Services
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _context;

        public UserService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByID(int id)
        {
            return await _context.Users.Where(u => u.IDUser == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByName(string name)
        {
            return await _context.Users.Where(u => u.Name == name).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> CheckIfUserExists(int? id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.IDUser == id);
            return user;
        }

        public async Task<User> EditUser(User user)
        {
            var result = await CheckIfUserExists(user.IDUser);
            result.IDUser = user.IDUser;
            result.Name = user.Name;
            result.Surname = user.Surname;
            result.PhoneNumber = user.PhoneNumber;

            _context.Users.Update(result);
            await(_context.SaveChangesAsync());
            return result;
        }
    }
}
