namespace Application.DTO.Auth
{
    public class LoginResponseDTO
    {
        public string AccessToken { get; set; }
        public string RefreshTokenId { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
