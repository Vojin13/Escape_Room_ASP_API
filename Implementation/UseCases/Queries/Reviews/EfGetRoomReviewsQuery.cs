using Application.DTO;
using Application.DTO.Reviews;
using Application.DTO.Search;
using Application.Extensions;
using Application.Queries.Reviews;
using System.Linq;

namespace Implementation.UseCases.Queries.Reviews
{
    public class EfGetRoomReviewsQuery : EfUseCase, IGetRoomReviewsQuery
    {
        public EfGetRoomReviewsQuery(AppDbContext context) : base(context)
        {
        }

        public string Name => "Get Room Reviews";

        public string Id => "get-room-reviews";

        public PagedResponse<ReviewDTO> Execute(ReviewSearchDTO request)
        {
            var query = _ctx.Reviews.Where(x => x.RoomId == request.RoomId);

            if(request.Rating.HasValue)
            {
                query = query.Where(x => x.Rating == request.Rating.Value);
            }

            return query
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new ReviewDTO
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Username = x.User.Username,
                    Rating = x.Rating,
                    Comment = x.Comment,
                    CreatedAt = x.CreatedAt
                }).ToPagedResponse(request.Page, request.PerPage);
        }
    }
}
