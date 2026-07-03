using Application.Commands.Timeslots;
using Application.DTO.Timeslot;
using Domain.Entities;
using FluentValidation;
using Implementation.UseCases.Validators.Timeslots;

namespace Implementation.UseCases.Commands.Timeslots.Admin
{
    public class EfCreateTimeslotCommand : EfUseCase, ICreateTimeslotCommand
    {
        private readonly CreateTimeslotValidator _validator;

        public EfCreateTimeslotCommand(AppDbContext context, CreateTimeslotValidator validator) : base(context)
        {
            _validator = validator;
        }

        public string Name => "Create Timeslot";

        public string Id => "create-timeslot";

        public void Execute(CreateTimeslotDTO data)
        {
            _validator.ValidateAndThrow(data);

            var timeslot = new Timeslot
            {
                StartTime = data.StartTime,
                EndTime = data.EndTime
            };

            _ctx.Timeslots.Add(timeslot);
            _ctx.SaveChanges();
        }
    }
}
