using System.ComponentModel.DataAnnotations;

namespace IOC.Models
{
    public class User
    {
        public User()
        {
            UserRefreshTokens = new HashSet<UserRefreshToken>();
        }
        public int IDUser { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string MailAddress { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Availability> Availabilities { get; } = new List<Availability>();
        public virtual ICollection<UserRefreshToken> UserRefreshTokens { get; set; }
        
    }
}
