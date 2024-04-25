namespace Souq.ViewModels.Account
{
    public class AuthanticationResponseDto
    {
        public string Message { get; set; }
        public bool IsAuthanticated { get; set; }

        // Token
        public string Token { get; set; }
        public DateTime? ExpiresOn { get; set; }

        // RefreshToken
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}