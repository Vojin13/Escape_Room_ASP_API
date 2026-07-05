using Application.DTO;
using Application.DTO.Users;

namespace Application.Queries.Users
{
    public interface IGetMyProfileQuery : IQuery<NoData, UserDetailsDTO>
    {
    }
}
