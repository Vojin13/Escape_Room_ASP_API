using Application.DTO;
using Application.Queries.Bookings;

namespace Implementation.UseCases.Queries.Bookings
{
    public class EfGetBookingStatusesQuery : EfUseCase, IGetBookingStatusesQuery
    {
        public EfGetBookingStatusesQuery(AppDbContext context) : base(context)
        {
        }

        public string Name => "Get Booking Statuses";

        public string Id => "get-booking-statuses";

        public IEnumerable<LookupDTO> Execute(NoData request)
        {
            return _ctx.BookingStatuses
                .Select(x => new LookupDTO { Id = x.Id, Name = x.Name })
                .ToList();
        }
    }
}
