using Application.Commands.Rooms;
using Application.DTO.Rooms;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Implementation.UseCases.Validators.Rooms;
using Microsoft.AspNetCore.Hosting;

namespace Implementation.UseCases.Commands.Rooms
{
    public class EfCreateRoomCommand : EfUseCase, ICreateRoomCommand
    {
        private readonly CreateRoomValidator _validator;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public EfCreateRoomCommand(AppDbContext context, CreateRoomValidator validator, IMapper mapper, IWebHostEnvironment env)
            : base(context)
        {
            _validator = validator;
            _mapper = mapper;
            _env = env;
        }

        public string Name => "Create Room";

        public string Id => "create-room";

        public void Execute(CreateRoomDTO data)
        {
            _validator.ValidateAndThrow(data);

            var room = _mapper.Map<Room>(data);
            room.IsActive = true;

            _ctx.Rooms.Add(room);
            _ctx.SaveChanges();

            data.TimeslotIds.ForEach(x =>
            {
                _ctx.RoomTimeslots.Add(new RoomTimeslot
                {
                    RoomId = room.Id,
                    TimeslotId = x
                });
            });

            var uploadsDir = Path.Combine(_env.WebRootPath, "images", "rooms", room.Id.ToString());
            Directory.CreateDirectory(uploadsDir);

            for (int i = 0; i < data.Images.Count; i++)
            {
                var file = data.Images[i];
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                var fileName = $"{Guid.NewGuid()}{extension}";
                var fullPath = Path.Combine(uploadsDir, fileName);

                using var stream = File.Create(fullPath);
                file.CopyTo(stream);

                _ctx.RoomImages.Add(new RoomImage
                {
                    RoomId = room.Id,
                    Path = $"/images/rooms/{room.Id}/{fileName}",
                    MimeType = file.ContentType,
                    Size = (int)file.Length,
                    IsPrimary = i == 0,
                    SortOrder = i
                });
            }

            _ctx.SaveChanges();
        }
    }
}
