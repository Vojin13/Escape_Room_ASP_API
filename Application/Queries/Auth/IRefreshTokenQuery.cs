using Application.DTO.Auth;

namespace Application.Queries.Auth
{
    public interface IRefreshTokenQuery : IQuery<RefreshTokenDTO, JwtTokenResponseDTO>
    {
    }
}
