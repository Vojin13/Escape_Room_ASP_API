using Application.DTO.Auth;

namespace Application.Commands.Auth
{
    public interface ILogoutUserCommand : ICommand<LogoutDTO>
    {
    }
}
