using Application.DTO.Rooms;

namespace Application.Queries.Rooms.Admin
{
    public interface IAdminGetRoomQuery : IQuery<int, RoomDetailsDTO>
    {
    }
}
