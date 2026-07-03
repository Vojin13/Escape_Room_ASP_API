using Application.Commands.Timeslots;
using Application.Exceptions;

namespace Implementation.UseCases.Commands.Timeslots.Admin
{
    public class EfDeleteTimeslotCommand : EfUseCase, IDeleteTimeslotCommand
    {
        public EfDeleteTimeslotCommand(AppDbContext context) : base(context)
        {
        }

        public string Name => "Delete Timeslot";

        public string Id => "delete-timeslot";

        public void Execute(int data)
        {
            var timeslot = _ctx.Timeslots.FirstOrDefault(x => x.Id == data);

            if (timeslot == null)
                throw new NotFoundException("Timeslot", data);

            var isInUse = _ctx.RoomTimeslots.Any(rt => rt.TimeslotId == data)
                       || _ctx.Bookings.Any(b => b.TimeslotId == data)
                       || _ctx.TimeslotLocks.Any(tl => tl.TimeslotId == data);

            if (isInUse)
                throw new ConflictException("Timeslot is in use and cannot be deleted.");

            _ctx.Timeslots.Remove(timeslot);
            _ctx.SaveChanges();
        }
    }
}
