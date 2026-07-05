using Application;
using Application.Commands.Bookings;
using Application.DTO.Bookings;
using Application.Exceptions;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implementation.UseCases.Commands.Bookings
{
    public class EfCancelBookingCommand : EfUseCase, ICancelBookingCommand
    {
        private readonly IApplicationUser _user;

        public EfCancelBookingCommand(AppDbContext context, IApplicationUser user) : base(context)
        {
            _user = user;
        }

        public string Name => "Cancel booking";

        public string Id => "cancel-booking";

        public void Execute(CancelBookingDTO data)
        {
            var booking = _ctx.Bookings.FirstOrDefault(x => x.Id == data.BookingId
                                                       && x.UserId == _user.Id);

            if(booking == null)
            {
                throw new NotFoundException("Booking", data.BookingId);
            }

            if(booking.StatusId == (int) BookingStatus.Cancelled ||
               booking.StatusId == (int) BookingStatus.Completed)
            {
                throw new ConflictException("Booking is already cancelled or completed.");
            }

            if (booking.BookingDate.Date < DateTime.UtcNow.Date)
            { 
                throw new ConflictException("Cannot cancel a booking for a past date.");
            }

            booking.StatusId = (int)BookingStatus.Cancelled;
            _ctx.SaveChanges();
        }
    }
}
