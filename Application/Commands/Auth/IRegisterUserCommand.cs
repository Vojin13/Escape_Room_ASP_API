using Application.DTO.Auth;

namespace Application.Commands.Auth
{
    public interface IRegisterUserCommand : ICommand<RegisterUserDTO>
    {
    }
}
