namespace Application.DTO.Auth
{
    public class JwtTokenResponseDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
