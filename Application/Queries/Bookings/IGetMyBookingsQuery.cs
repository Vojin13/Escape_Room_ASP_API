using Application.DTO;
using Application.DTO.Bookings;
using Application.DTO.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Bookings
{
    public interface IGetMyBookingsQuery : IQuery<MyBookingSearchDTO, PagedResponse<BookingDTO>>
    {
    }
}
