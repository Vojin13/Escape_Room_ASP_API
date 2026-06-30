using Application.DTO.Rooms;
using Application.DTO.Timeslot;
using Application.Exceptions;
using Application.Queries.Rooms;

namespace Implementation.UseCases.Queries.Rooms
{
    public class EfGetRoomAvailabilityQuery : EfUseCase, IGetRoomAvailabilityQuery
    {
        public EfGetRoomAvailabilityQuery(AppDbContext context) : base(context)
        {
        }

        public string Name => "Get Room Availability";

        public string Id => "get-room-availability";

        public IEnumerable<TimeslotAvailabilityDTO> Execute(RoomAvailabilityDTO request)
        {
            var date = DateTime.SpecifyKind(request.Date.Date, DateTimeKind.Utc);

            var room = _ctx.Rooms.FirstOrDefault(r => r.Id == request.RoomId && r.IsActive);

            if (room == null)
                throw new NotFoundException("Room", request.RoomId);

            var timeslots = _ctx.RoomTimeslots
                .Where(rt => rt.RoomId == request.RoomId)
                .Select(rt => rt.Timeslot)
                .ToList();

            return timeslots
                .Select(t => new TimeslotAvailabilityDTO
                {
                    TimeslotId = t.Id,
                    StartTime = t.StartTime.ToString(@"HH\:mm"),
                    EndTime = t.EndTime.ToString(@"HH\:mm")
                });
        }
    }
}
