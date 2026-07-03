using Application.DTO;
using Application.DTO.Search;
using Application.DTO.Timeslot;

namespace Application.Queries.Timeslots.Admin
{
    public interface IAdminGetTimeslotsQuery : IQuery<TimeslotSearchDTO, PagedResponse<TimeslotDTO>>
    {
    }
}
