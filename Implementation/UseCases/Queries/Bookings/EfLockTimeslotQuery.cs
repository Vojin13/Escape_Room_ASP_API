using Application.DTO.Bookings;
using Application.Exceptions;
using Application.Queries.Bookings;
using Domain.Entities;
using Domain.Enums;
using System.Linq;

namespace Implementation.UseCases.Queries.Bookings
{
    public class EfLockTimeslotQuery : EfUseCase, ILockTimeslotQuery
    {
        public EfLockTimeslotQuery(AppDbContext context) : base(context)
        {
        }

        public string Name => "Lock Timeslot";

        public string Id => "lock-timeslot";

        public TimeslotLockResponseDTO Execute(LockTimeslotDTO request)
        {
            var date = DateTime.SpecifyKind(request.Date.Date, DateTimeKind.Utc);

            var room = _ctx.Rooms.FirstOrDefault(x => x.Id == request.RoomId && x.IsActive);

            if(room == null)
            {
                throw new NotFoundException("Room", request.RoomId);
            }

            var roomTimeslot = _ctx.RoomTimeslots.FirstOrDefault(x => x.TimeslotId == request.TimeslotId && x.RoomId == request.RoomId);

            if(roomTimeslot == null)
            {
                throw new NotFoundException("Timeslot", request.TimeslotId);
            }

            var booking = _ctx.Bookings
                .FirstOrDefault(x => x.RoomId == request.RoomId 
                && x.TimeslotId == request.TimeslotId 
                && x.BookingDate == date
                && (x.StatusId == (int)BookingStatus.Pending || x.StatusId == (int)BookingStatus.Confirmed));

            if(booking != null)
            {
                throw new ConflictException("Timeslot is already booked.");
            }

            var now = DateTime.UtcNow;

            var existingLock = _ctx.TimeslotLocks
                .FirstOrDefault(x => x.RoomId == request.RoomId
                && x.TimeslotId == request.TimeslotId
                && x.Date == date && x.ExpiresAt > now);

            if(existingLock != null && existingLock.UserId != request.UserId)
            {
                throw new ConflictException("Timeslot is already locked by another user.");
            }

            // A user can only hold one active lock at a time — picking a different slot
            // releases whatever they were previously holding.
            var otherLocks = _ctx.TimeslotLocks
                .Where(x => x.UserId == request.UserId && x.ExpiresAt > now
                && !(x.RoomId == request.RoomId && x.TimeslotId == request.TimeslotId && x.Date == date))
                .ToList();

            if(otherLocks.Any())
            {
                _ctx.TimeslotLocks.RemoveRange(otherLocks);
            }

            if(existingLock != null)
            {
                existingLock.ExpiresAt = now.AddMinutes(5);

                _ctx.SaveChanges();

                return new TimeslotLockResponseDTO
                {
                    LockId = existingLock.Id,
                    ExpiresAt = existingLock.ExpiresAt,
                };
            }

            TimeslotLock tlock = new TimeslotLock
            {
                RoomId = request.RoomId,
                Date = date,
                ExpiresAt = now.AddMinutes(5),
                UserId = request.UserId,
                TimeslotId = request.TimeslotId,
            };

            _ctx.TimeslotLocks.Add(tlock);
            _ctx.SaveChanges();

            return new TimeslotLockResponseDTO
            {
                LockId = tlock.Id,
                ExpiresAt = tlock.ExpiresAt,
            };
        }
    }
}
