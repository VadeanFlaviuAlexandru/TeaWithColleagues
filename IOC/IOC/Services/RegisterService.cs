using IOC.DataBase;
using IOC.Models;
using IOC.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IOC.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly DatabaseContext _context;

        public RegisterService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckIfUserExists(User user)
        {
            var userMail = await _context.Users.FirstOrDefaultAsync(u => u.MailAddress == user.MailAddress);
            if (userMail == null)
            {
                if (user.Password != null)
                {
                    user.Password = Hasher.CreateMD5(user.Password);
                    var addedUser = await AddUserToDatabase(user);
                }
            }
            else
                return true;

            return false;
        }

        public async Task<bool> AddUserToDatabase(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
