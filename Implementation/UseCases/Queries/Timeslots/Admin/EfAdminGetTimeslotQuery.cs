using Application.DTO.Timeslot;
using Application.Exceptions;
using Application.Queries.Timeslots.Admin;

namespace Implementation.UseCases.Queries.Timeslots.Admin
{
    public class EfAdminGetTimeslotQuery : EfUseCase, IAdminGetTimeslotQuery
    {
        public EfAdminGetTimeslotQuery(AppDbContext context) : base(context)
        {
        }

        public string Name => "Admin Get Timeslot";

        public string Id => "get-timeslot";

        public TimeslotDTO Execute(int request)
        {
            var timeslot = _ctx.Timeslots.FirstOrDefault(x => x.Id == request);

            if (timeslot == null)
                throw new NotFoundException("Timeslot", request);

            return new TimeslotDTO
            {
                Id = timeslot.Id,
                StartTime = timeslot.StartTime.ToString(@"HH\:mm"),
                EndTime = timeslot.EndTime.ToString(@"HH\:mm")
            };
        }
    }
}
