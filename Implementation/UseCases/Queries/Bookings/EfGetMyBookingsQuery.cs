using Application;
using Application.DTO;
using Application.DTO.Bookings;
using Application.DTO.Search;
using Application.Extensions;
using Application.Queries.Bookings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Implementation.UseCases.Queries.Bookings
{
    public class EfGetMyBookingsQuery : EfUseCase, IGetMyBookingsQuery
    {
        private readonly IApplicationUser _user;

        public EfGetMyBookingsQuery(AppDbContext context, IApplicationUser user) : base(context)
        {
            _user = user;
        }

        public string Name => "Get My Bookings";

        public string Id => "get-my-bookings";

        public PagedResponse<BookingDTO> Execute(MyBookingSearchDTO request)
        {
            var query = _ctx.Bookings
                                .Where(x => x.UserId == _user.Id)
                                .AsQueryable();

            if (request.StatusId.HasValue)
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
