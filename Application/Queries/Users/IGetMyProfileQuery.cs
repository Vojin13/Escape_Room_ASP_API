using Application.DTO.Users;

namespace Application.Queries.Users
{
    public interface IGetMyProfileQuery : IQuery<int, UserDetailsDTO>
    {
    }
}
