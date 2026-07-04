using Application.DTO.Bookings;
using FluentValidation;
using System.Linq;

namespace Implementation.UseCases.Validators.Bookings
{
    public class UpdateBookingStatusValidator : BaseValidator<UpdateBookingStatusDTO>
    {
        public UpdateBookingStatusValidator(AppDbContext ctx)
        {
            this.RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.StatusId)
                .Must(id => ctx.BookingStatuses.Any(s => s.Id == id))
                .WithMessage("Invalid booking status.");
        }
    }
}
