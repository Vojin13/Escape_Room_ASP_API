using Application.Commands.Timeslots;
using Application.DTO.Timeslot;
using Application.Exceptions;
using FluentValidation;
using Implementation.UseCases.Validators.Timeslots;

namespace Implementation.UseCases.Commands.Timeslots.Admin
{
    public class EfUpdateTimeslotCommand : EfUseCase, IUpdateTimeslotCommand
    {
        private readonly UpdateTimeslotValidator _validator;

        public EfUpdateTimeslotCommand(AppDbContext context, UpdateTimeslotValidator validator) : base(context)
        {
            _validator = validator;
        }

        public string Name => "Update Timeslot";

        public string Id => "update-timeslot";

        public void Execute(UpdateTimeslotDTO data)
        {
            _validator.ValidateAndThrow(data);

            var timeslot = _ctx.Timeslots.FirstOrDefault(x => x.Id == data.Id);

            if (timeslot == null)
                throw new NotFoundException("Timeslot", data.Id);

            timeslot.StartTime = data.StartTime;
            timeslot.EndTime = data.EndTime;

            _ctx.SaveChanges();
        }
    }
}
