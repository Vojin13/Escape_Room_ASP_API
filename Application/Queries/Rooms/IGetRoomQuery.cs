using Application.DTO.Rooms;

namespace Application.Queries.Rooms
{
    public interface IGetRoomQuery : IQuery<int, RoomDetailsDTO>
    {
    }
}
