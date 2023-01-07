namespace IOC.Models
{
    public class AuthenticatedResponse
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public int IDUser { get; set; }
    }
}
