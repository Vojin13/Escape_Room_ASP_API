using Application.DTO.Bookings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Bookings
{
    public interface ICancelBookingCommand : ICommand<CancelBookingDTO>
    {
    }
}
