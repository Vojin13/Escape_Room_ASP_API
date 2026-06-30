using Application.Commands.Rooms;
using Application.DTO.Rooms;
using Application.Exceptions;
using Domain.Entities;
using FluentValidation;
using Implementation.UseCases.Validators.Rooms;

namespace Implementation.UseCases.Commands.Rooms
{
    public class EfUpdateRoomCommand : EfUseCase, IUpdateRoomCommand
    {
        private readonly UpdateRoomValidator _validator;

        public EfUpdateRoomCommand(AppDbContext context, UpdateRoomValidator validator) : base(context)
        {
            _validator = validator;
        }

        public string Name => "Update Room";

        public string Id => "update-room";

        public void Execute(UpdateRoomDTO data)
        {
            _validator.ValidateAndThrow(data);

            var room = _ctx.Rooms.FirstOrDefault(r => r.Id == data.Id);

            if (room == null)
                throw new NotFoundException("Room", data.Id);

            room.Title = data.Title;
            room.Description = data.Description;
            room.MinimumPlayers = data.MinimumPlayers;
            room.MaximumPlayers = data.MaximumPlayers;
            room.DurationInMinutes = data.DurationInMinutes;
            room.PricePerPerson = data.PricePerPerson;
            room.DifficultyId = data.DifficultyId;

            var existing = _ctx.RoomTimeslots.Where(rt => rt.RoomId == data.Id).ToList();
            _ctx.RoomTimeslots.RemoveRange(existing);

            _ctx.RoomTimeslots.AddRange(data.TimeslotIds.Select(tid => new RoomTimeslot
            {
                RoomId = data.Id,
                TimeslotId = tid
            }));

            _ctx.SaveChanges();
        }
    }
}
