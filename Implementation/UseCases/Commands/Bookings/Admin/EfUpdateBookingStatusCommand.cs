using Application.Commands.Bookings;
using Application.DTO.Bookings;
using Application.Exceptions;
using FluentValidation;
using Implementation.UseCases.Validators.Bookings;
using System.Linq;

namespace Implementation.UseCases.Commands.Bookings.Admin
{
    public class EfUpdateBookingStatusCommand : EfUseCase, IUpdateBookingStatusCommand
    {
        private readonly UpdateBookingStatusValidator _validator;

        public EfUpdateBookingStatusCommand(AppDbContext context, UpdateBookingStatusValidator validator) : base(context)
        {
            _validator = validator;
        }

        public string Name => "Update Booking Status";

        public string Id => "update-booking-status";

        public void Execute(UpdateBookingStatusDTO data)
        {
            _validator.ValidateAndThrow(data);

            var booking = _ctx.Bookings.FirstOrDefault(x => x.Id == data.Id);

            if(booking == null)
            {
                throw new NotFoundException("Booking", data.Id);
            }

            booking.StatusId = data.StatusId;

            _ctx.SaveChanges();
        }
    }
}
