using Application.DTO;
using Application.DTO.Bookings;
using Application.DTO.Search;
using Application.Extensions;
using Application.Queries.Bookings.Admin;
using System.Linq;

namespace Implementation.UseCases.Queries.Bookings.Admin
{
    public class EfAdminGetBookingsQuery : EfUseCase, IAdminGetBookingsQuery
    {
        public EfAdminGetBookingsQuery(AppDbContext context) : base(context)
        {
        }

        public string Name => "Admin Get Bookings";

        public string Id => "get-all-bookings";

        public PagedResponse<BookingDTO> Execute(BookingSearchDTO request)
        {
            var query = _ctx.Bookings.AsQueryable();

            query = query.WhereContainsIgnoreCaseAny(request.Keyword,
                x => x.Room.Title, x => x.User.Username, x => x.User.Email);

            if(request.StatusId.HasValue)
            {
                query = query.Where(x => x.StatusId == request.StatusId.Value);
            }

            query = request.SortDescending
                ? query.OrderByDescending(x => x.CreatedAt)
                : query.OrderBy(x => x.CreatedAt);

            return query.Select(x => new BookingDTO
            {
                Id = x.Id,
                RoomId = x.RoomId,
                RoomTitle = x.Room.Title,
                UserId = x.UserId,
                Username = x.User.Username,
                TimeslotId = x.TimeslotId,
                StartTime = x.Timeslot.StartTime.ToString("HH:mm"),
                EndTime = x.Timeslot.EndTime.ToString("HH:mm"),
                BookingDate = x.BookingDate,
                NumberOfPlayers = x.NumberOfPlayers,
                TotalPrice = x.TotalPrice,
                StatusId = x.StatusId,
                Status = x.Status.Name,
                CreatedAt = x.CreatedAt
            }).ToPagedResponse(request.Page, request.PerPage);
        }
    }
}
