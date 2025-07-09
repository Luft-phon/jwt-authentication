namespace JwtAuthentication.Models
{
    public class RefreshTokenRequestDTO
    {
        public Guid UserID { get; set; }
        public required string RefreshToken { get; set; }
    }
}
