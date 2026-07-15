using Application;
using Application.DTO;
using Application.DTO.Bookings;
using Application.Queries.Bookings;
using System.Linq;

namespace Implementation.UseCases.Queries.Bookings
{
    public class EfGetMyCartQuery : EfUseCase, IGetMyCartQuery
    {
        private readonly IApplicationUser _user;

        public EfGetMyCartQuery(AppDbContext context, IApplicationUser user) : base(context)
        {
            _user = user;
        }

        public string Name => "Get My Cart";

        public string Id => "get-my-cart";

        public MyCartDTO Execute(NoData data)
        {
            var now = DateTime.UtcNow;

            var activeLock = _ctx.TimeslotLocks
                .FirstOrDefault(x => x.UserId == _user.Id && x.ExpiresAt > now);

            if (activeLock == null)
            {
                return null;
            }

            return new MyCartDTO
            {
                LockId = activeLock.Id,
                RoomId = activeLock.RoomId,
                RoomTitle = activeLock.Room.Title,
                PrimaryImage = activeLock.Room.Images.FirstOrDefault(i => i.IsPrimary)?.Path,
                TimeslotId = activeLock.TimeslotId,
                StartTime = activeLock.Timeslot.StartTime.ToString(@"HH\:mm"),
                EndTime = activeLock.Timeslot.EndTime.ToString(@"HH\:mm"),
                Date = activeLock.Date,
                PricePerPerson = activeLock.Room.PricePerPerson,
                ExpiresAt = activeLock.ExpiresAt
            };
        }
    }
}
