using Application.DTO.Rooms;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implementation.UseCases.Validators.Rooms
{
    public class CreateRoomValidator : BaseValidator<CreateRoomDTO>
    {
        private readonly AppDbContext _ctx;

        public CreateRoomValidator(AppDbContext ctx) 
        {
            _ctx = ctx;
            this.RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Title)
            .NotEmpty().WithMessage(Required)
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.")
            .Must(title => !_ctx.Rooms.Any(r => r.Title == title))
            .WithMessage("A room with this title already exists."); ;

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
                .Must(AllTimeslotsExist)
                .WithMessage("One or more timeslots are invalid.");

            RuleFor(x => x.Images)
                .NotEmpty().WithMessage("Room must have at least one image.");
        }

        private bool AllTimeslotsExist(List<int> timeslotIds)
        {
            var existingCount = _ctx.Timeslots.Count(t => timeslotIds.Contains(t.Id));
            return existingCount == timeslotIds.Count;
        }
    }
}
