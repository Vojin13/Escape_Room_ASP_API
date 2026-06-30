using Application.DTO;
using Application.DTO.Rooms;
using Application.DTO.Search;

namespace Application.Queries.Rooms.Admin
{
    public interface IAdminGetRoomsQuery : IQuery<RoomSearchDTO, PagedResponse<RoomDTO>>
    {
    }
}
