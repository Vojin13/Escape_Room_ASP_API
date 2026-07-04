using Application.DTO.Bookings;
using Application.Exceptions;
using Application.Queries.Bookings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implementation.UseCases.Queries.Bookings
{
    public class EfGetBookingQuery : EfUseCase, IGetBookingQuery
    {
        public EfGetBookingQuery(AppDbContext context) : base(context)
        {
        }

        public string Name => "Get Booking";

        public string Id => "get-booking";

        public BookingDTO Execute(GetBookingDTO request)
        {
            var booking = _ctx.Bookings.FirstOrDefault(x => x.Id == request.BookingId
                                                       && x.UserId == request.UserId);

            if(booking == null)
            {
                throw new NotFoundException("Booking", request.BookingId);
            }

            return new BookingDTO
            {
                Id = booking.Id,
                RoomId = booking.RoomId,
                RoomTitle = booking.Room.Title,
                UserId = booking.UserId,
                Username = booking.User.Username,
                TimeslotId = booking.TimeslotId,
                StartTime = booking.Timeslot.StartTime.ToString("HH:mm"),
                EndTime = booking.Timeslot.EndTime.ToString("HH:mm"),
                BookingDate = booking.BookingDate,
                NumberOfPlayers = booking.NumberOfPlayers,
                TotalPrice = booking.TotalPrice,
                StatusId = booking.StatusId,
                Status = booking.Status.Name,
                CreatedAt = booking.CreatedAt
            };
        }
    }
}
