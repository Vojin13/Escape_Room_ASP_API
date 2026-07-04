using Application.DTO;

namespace Application.Queries.Bookings
{
    public interface IGetBookingStatusesQuery : IQuery<NoData, IEnumerable<LookupDTO>>
    {
    }
}
