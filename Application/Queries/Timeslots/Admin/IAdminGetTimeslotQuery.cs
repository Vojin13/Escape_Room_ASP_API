using Application.DTO.Timeslot;

namespace Application.Queries.Timeslots.Admin
{
    public interface IAdminGetTimeslotQuery : IQuery<int, TimeslotDTO>
    {
    }
}
