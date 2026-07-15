using Application;
using Application.Commands.Bookings;
using Application.DTO;
using Application.Exceptions;
using System.Linq;

namespace Implementation.UseCases.Commands.Bookings
{
    public class EfCancelLockCommand : EfUseCase, ICancelLockCommand
    {
        private readonly IApplicationUser _user;

        public EfCancelLockCommand(AppDbContext context, IApplicationUser user) : base(context)
        {
            _user = user;
        }

        public string Name => "Cancel Timeslot Lock";

        public string Id => "cancel-lock";

        public void Execute(NoData data)
        {
            var now = DateTime.UtcNow;

            var activeLock = _ctx.TimeslotLocks
                .FirstOrDefault(x => x.UserId == _user.Id && x.ExpiresAt > now);

            if (activeLock == null)
            {
                throw new NotFoundException("Active timeslot lock", _user.Id);
            }

            _ctx.TimeslotLocks.Remove(activeLock);
            _ctx.SaveChanges();
        }
    }
}
