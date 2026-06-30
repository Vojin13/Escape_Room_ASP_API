using Application.DTO.Rooms;
using Application.DTO.Timeslot;
using Application.Exceptions;
using Application.Queries.Rooms.Admin;
using Microsoft.EntityFrameworkCore;

namespace Implementation.UseCases.Queries.Rooms.Admin
{
    public class EfAdminGetRoomQuery : EfUseCase, IAdminGetRoomQuery
    {
        public EfAdminGetRoomQuery(AppDbContext context) : base(context)
        {
        }

        public string Name => "Admin Get Room";

        public string Id => "admin-get-room";

        public RoomDetailsDTO Execute(int id)
        {
            var room = _ctx.Rooms
                           .Include(x => x.Images)
                           .Include(x => x.Reviews)
                           .Include(x => x.Difficulty)
                           .Include(x => x.RoomTimeslots)
                           .ThenInclude(x => x.Timeslot)
                           .FirstOrDefault(x => x.Id == id);

            if (room == null)
                throw new NotFoundException("Room", id);

            return new RoomDetailsDTO
            {
                Id = room.Id,
                Title = room.Title,
                Description = room.Description,
                Difficulty = room.Difficulty.Name,
                DurationInMinutes = room.DurationInMinutes,
                Images = room.Images.OrderByDescending(x => x.IsPrimary)
                                    .ThenBy(x => x.SortOrder)
                                    .Select(x => x.Path)
                                    .ToList(),
                Timeslots = room.RoomTimeslots.Select(x => new TimeslotDTO
                {
                    Id = x.TimeslotId,
                    StartTime = x.Timeslot.StartTime.ToString("HH:mm"),
                    EndTime = x.Timeslot.EndTime.ToString("HH:mm")
                }).ToList(),
                MinimumPlayers = room.MinimumPlayers,
                MaximumPlayers = room.MaximumPlayers,
                PricePerPerson = room.PricePerPerson,
                ReviewCount = room.Reviews.Count,
                AverageRating = room.Reviews.Any() ? room.Reviews.Average(x => x.Rating) : 0,
            };
        }
    }
}
