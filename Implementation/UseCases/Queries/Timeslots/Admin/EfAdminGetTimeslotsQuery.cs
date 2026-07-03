using Application.DTO;
using Application.DTO.Search;
using Application.DTO.Timeslot;
using Application.Extensions;
using Application.Queries.Timeslots.Admin;

namespace Implementation.UseCases.Queries.Timeslots.Admin
{
    public class EfAdminGetTimeslotsQuery : EfUseCase, IAdminGetTimeslotsQuery
    {
        public EfAdminGetTimeslotsQuery(AppDbContext context) : base(context)
        {
        }

        public string Name => "Admin Get Timeslots";

        public string Id => "get-timeslots";

        public PagedResponse<TimeslotDTO> Execute(TimeslotSearchDTO request)
        {
            return _ctx.Timeslots
                .OrderBy(x => x.StartTime)
                .Select(x => new TimeslotDTO
                {
                    Id = x.Id,
                    StartTime = x.StartTime.ToString(@"HH\:mm"),
                    EndTime = x.EndTime.ToString(@"HH\:mm")
                }).ToPagedResponse(request.Page, request.PerPage);
        }
    }
}
