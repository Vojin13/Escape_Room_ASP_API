using Application.DTO.Auth;

namespace Application.Queries.Auth
{
    public interface ILoginUserQuery : IQuery<LoginDTO, JwtTokenResponseDTO>
    {
    }
}
