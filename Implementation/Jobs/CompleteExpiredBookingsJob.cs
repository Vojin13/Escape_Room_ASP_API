using Application.Jobs;
using Domain.Enums;
using System.Linq;

namespace Implementation.Jobs
{
    public class CompleteExpiredBookingsJob : ICompleteExpiredBookingsJob
    {
        private readonly AppDbContext _ctx;

        public CompleteExpiredBookingsJob(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public void Execute()
        {
            var today = DateTime.UtcNow.Date;

            var bookingsToComplete = _ctx.Bookings
                .Where(x => x.StatusId == (int)BookingStatus.Confirmed && x.BookingDate.Date < today)
                .ToList();

            foreach (var booking in bookingsToComplete)
            {
                booking.StatusId = (int)BookingStatus.Completed;
            }

            _ctx.SaveChanges();
        }
    }
}
