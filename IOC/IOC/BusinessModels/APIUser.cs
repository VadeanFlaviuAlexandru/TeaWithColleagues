using System.ComponentModel.DataAnnotations;

namespace IOC.Models
{
    public class APIUser
    {
        public int IDUser { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? PhoneNumber { get; set; }
        public string? MailAddress { get; set; }        
    }
}
