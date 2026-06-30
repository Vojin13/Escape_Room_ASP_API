using Application.DTO.Rooms;
using Application.DTO.Timeslot;

namespace Application.Queries.Rooms
{
    public interface IGetRoomAvailabilityQuery : IQuery<RoomAvailabilityDTO, IEnumerable<TimeslotAvailabilityDTO>>
    {
    }
}
