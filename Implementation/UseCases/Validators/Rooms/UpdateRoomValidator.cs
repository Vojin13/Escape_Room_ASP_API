using Application.DTO.Rooms;
using FluentValidation;

namespace Implementation.UseCases.Validators.Rooms
{
    public class UpdateRoomValidator : BaseValidator<UpdateRoomDTO>
    {
        public UpdateRoomValidator(AppDbContext ctx)
        {
            this.RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(Required)
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.")
                .Must((dto, title) => !ctx.Rooms.Any(r => r.Title == title && r.Id != dto.Id))
                .WithMessage("A room with this title already exists.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(Required);

            RuleFor(x => x.MinimumPlayers)
                .GreaterThan((short)0).WithMessage("Minimum players must be greater than 0.");

            RuleFor(x => x.MaximumPlayers)
                .GreaterThan(x => x.MinimumPlayers)
                .WithMessage("Maximum players must be greater than minimum players.");

            RuleFor(x => x.DurationInMinutes)
                .Must(d => d == 45 || d == 60 || d == 90)
                .WithMessage("Duration must be 45, 60 or 90 minutes.");

            RuleFor(x => x.PricePerPerson)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.DifficultyId)
                .Must(id => ctx.Difficulties.Any(d => d.Id == id))
                .WithMessage("Invalid difficulty.");

            RuleFor(x => x.TimeslotIds)
                .NotEmpty().WithMessage("Room must have at least one timeslot.")
                .Must(ids => ctx.Timeslots.Count(t => ids.Contains(t.Id)) == ids.Count)
                .WithMessage("One or more timeslots are invalid.");
        }
    }
}
