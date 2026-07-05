using Application.DTO.Bookings;
using FluentValidation;
using System;
using System.Linq;

namespace Implementation.UseCases.Validators.Bookings
{
    public class CreateBookingValidator : BaseValidator<CreateBookingDTO>
    {
        private readonly AppDbContext _ctx;

        public CreateBookingValidator(AppDbContext ctx)
        {
            this.RuleLevelCascadeMode = CascadeMode.Stop;
            _ctx = ctx;

            RuleFor(x => x.RoomId)
                .NotEmpty().WithMessage(Required);

            RuleFor(x => x.TimeslotId)
                .NotEmpty().WithMessage(Required);

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage(Required)
                .Must(x => x.Date >= DateTime.UtcNow.Date)
                .WithMessage("Booking date cannot be in the past.");

            RuleFor(x => x.NumberOfPlayers)
                .NotEmpty().WithMessage(Required)
                .Must(BeWithinRoomCapacity)
                .WithMessage("Number of players is outside the allowed range for this room.");
        }

        private bool BeWithinRoomCapacity(CreateBookingDTO dto, int numberOfPlayers)
        {
            var room = _ctx.Rooms.FirstOrDefault(r => r.Id == dto.RoomId);

            return room != null && numberOfPlayers >= room.MinimumPlayers && numberOfPlayers <= room.MaximumPlayers;
        }
    }
}
