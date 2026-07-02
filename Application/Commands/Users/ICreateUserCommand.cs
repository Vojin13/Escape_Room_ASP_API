using Application.DTO.Users;

namespace Application.Commands.Users
{
    public interface ICreateUserCommand : ICommand<CreateUserDTO>
    {
    }
}
