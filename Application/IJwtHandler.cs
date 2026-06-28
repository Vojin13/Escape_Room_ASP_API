using Application.DTO.Auth;
using Domain.Entities;

namespace Application
{
    public interface IJwtHandler
    {
        JwtTokenResponseDTO MakeToken(User user);
    }
}
