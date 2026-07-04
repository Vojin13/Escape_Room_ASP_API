using Application.DTO;
using Application.DTO.Bookings;
using Application.DTO.Search;

namespace Application.Queries.Bookings.Admin
{
    public interface IAdminGetBookingsQuery : IQuery<BookingSearchDTO, PagedResponse<BookingDTO>>
    {
    }
}
