namespace IOC.Models
{
    public partial class UserRefreshToken
    {
        public int IDUserRefreshToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public int IDUser { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
