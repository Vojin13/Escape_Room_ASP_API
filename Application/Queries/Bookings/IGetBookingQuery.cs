using Application.DTO.Bookings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Bookings
{
    public interface IGetBookingQuery : IQuery<GetBookingDTO, BookingDTO>
    {
    }
}
