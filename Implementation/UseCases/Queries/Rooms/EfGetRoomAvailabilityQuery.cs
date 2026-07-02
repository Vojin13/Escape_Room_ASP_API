using Application.DTO.Rooms;
using Application.DTO.Timeslot;
using Application.Exceptions;
using Application.Queries.Rooms;
using Domain.Enums;

namespace Implementation.UseCases.Queries.Rooms
{
    public class EfGetRoomAvailabilityQuery : EfUseCase, IGetRoomAvailabilityQuery
    {
        public EfGetRoomAvailabilityQuery(AppDbContext context) : base(context)
        {
        }

        public string Name => "Get Room Availability";

        public string Id => "get-room-availability";

        public IEnumerable<TimeslotDTO> Execute(RoomAvailabilityDTO request)
        {
            var date = DateTime.SpecifyKind(request.Date.Date, DateTimeKind.Utc);
            var nextDay = date.AddDays(1);

            var room = _ctx.Rooms.FirstOrDefault(r => r.Id == request.RoomId && r.IsActive);

            if (room == null)
                throw new NotFoundException("Room", request.RoomId);

            var timeslots = _ctx.RoomTimeslots
                .Where(rt => rt.RoomId == request.RoomId)
                .Select(rt => rt.Timeslot)
                .ToList();

            var bookedTimeslotIds = _ctx.Bookings
                .Where(b => b.RoomId == request.RoomId
                         && b.BookingDate >= date && b.BookingDate < nextDay
                         && (b.StatusId == (int)BookingStatus.Pending || b.StatusId == (int)BookingStatus.Confirmed))
                .Select(b => b.TimeslotId)
                .ToList();

            var now = DateTime.UtcNow;
            var lockedTimeslotIds = _ctx.TimeslotLocks
                .Where(tl => tl.RoomId == request.RoomId
                          && tl.Date >= date && tl.Date < nextDay
                          && tl.ExpiresAt > now)
                .Select(tl => tl.TimeslotId)
                .ToList();

            return timeslots
                .Where(t => !bookedTimeslotIds.Contains(t.Id) && !lockedTimeslotIds.Contains(t.Id))
                .Select(t => new TimeslotDTO
                {
                    Id = t.Id,
                    StartTime = t.StartTime.ToString("HH:mm"),
                    EndTime = t.EndTime.ToString("HH:mm")
                });
        }
    }
}
