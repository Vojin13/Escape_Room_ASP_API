using Application.DTO;
using Application.DTO.Rooms;
using Application.DTO.Search;
using Application.Extensions;
using Application.Queries.Rooms;
using Microsoft.EntityFrameworkCore;

namespace Implementation.UseCases.Queries.Rooms
{
    public class EfGetRoomsQuery : EfUseCase, IGetRoomsQuery
    {
        public EfGetRoomsQuery(AppDbContext context) : base(context)
        {
        }

        public string Name => "Get all active rooms";

        public string Id => "get-rooms";

        public PagedResponse<RoomDTO> Execute(RoomSearchDTO request)
        {
            var query = _ctx.Rooms
                .Include(x => x.Difficulty)
                .Include(x => x.Reviews)
                .Include(x => x.Images)
                .Where(x => x.IsActive)
                .AsQueryable();

            query = query.WhereContainsIgnoreCase(x => x.Title, request.Title);

            if (request.DifficultyId.HasValue)
            {
                query = query.Where(x => x.DifficultyId == request.DifficultyId);
            }

            if (request.MinPrice.HasValue)
            {
                query = query.Where(r => r.PricePerPerson >= request.MinPrice);
            }

            if (request.MaxPrice.HasValue)
            {
                query = query.Where(r => r.PricePerPerson <= request.MaxPrice);
            }

            if (request.PlayersCount.HasValue)
            {
                query = query.Where(r => r.MinimumPlayers <= request.PlayersCount &&
                                         r.MaximumPlayers >= request.PlayersCount);
            }

            if (request.DurationInMinutes.HasValue)
            {
                query = query.Where(r => r.DurationInMinutes == request.DurationInMinutes);
            }

            return query
                .OrderBy(x => x.Id)
                .Select(x => new RoomDTO
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Difficulty = x.Difficulty.Name,
                    DurationInMinutes = x.DurationInMinutes,
                    PrimaryImage = x.Images.Where(y => y.IsPrimary)
                                       .Select(y => y.Path)
                                       .FirstOrDefault(),
                    MinimumPlayers = x.MinimumPlayers,
                    MaximumPlayers = x.MaximumPlayers,
                    PricePerPerson = x.PricePerPerson,
                    ReviewCount = x.Reviews.Count(),
                    AverageRating = x.Reviews.Any() ? x.Reviews.Average(y => (double)y.Rating) : 0,
                    IsActive = x.IsActive,
                }).ToPagedResponse(request.Page, request.PerPage);

        }
    }
}

