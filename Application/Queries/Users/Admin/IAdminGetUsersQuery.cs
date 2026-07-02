using Application.DTO;
using Application.DTO.Search;
using Application.DTO.Users;

namespace Application.Queries.Users.Admin
{
    public interface IAdminGetUsersQuery : IQuery<UserSearchDTO, PagedResponse<UserDTO>>
    {
    }
}
