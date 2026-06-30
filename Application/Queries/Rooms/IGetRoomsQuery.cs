using Application.DTO;
using Application.DTO.Rooms;
using Application.DTO.Search;

namespace Application.Queries.Rooms
{
    public interface IGetRoomsQuery : IQuery<RoomSearchDTO, PagedResponse<RoomDTO>>
    {
    }
}
