using Application.DTO.Users;

namespace Application.Queries.Users.Admin
{
    public interface IAdminGetUserQuery : IQuery<int, UserDetailsDTO>
    {
    }
}
