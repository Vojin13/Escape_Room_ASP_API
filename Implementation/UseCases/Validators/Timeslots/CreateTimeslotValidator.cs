using Application.DTO.Timeslot;
using FluentValidation;

namespace Implementation.UseCases.Validators.Timeslots
{
    public class CreateTimeslotValidator : BaseValidator<CreateTimeslotDTO>
    {
        public CreateTimeslotValidator(AppDbContext ctx)
        {
            this.RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.EndTime)
                .GreaterThan(x => x.StartTime)
                .WithMessage("End time must be after start time.");

            RuleFor(x => x)
                .Must(dto => !ctx.Timeslots.Any(t => t.StartTime == dto.StartTime && t.EndTime == dto.EndTime))
                .WithMessage("A timeslot with this start and end time already exists.");
        }
    }
}
