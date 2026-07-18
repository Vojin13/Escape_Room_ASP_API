using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application;
using Application.Commands.Rooms;
using Application.Exceptions;

namespace Implementation.UseCases.Commands.Rooms
{
    public class EfDeleteRoomCommand : EfUseCase, IDeleteRoomCommand
    {
        private readonly ICacheService _cache;

        public EfDeleteRoomCommand(AppDbContext context, ICacheService cache) : base(context)
        {
            _cache = cache;
        }

        public string Name => "Delete Room";

        public string Id => "delete-room";

        public void Execute(int data)
        {
            var room = _ctx.Rooms.FirstOrDefault(x => x.IsActive && x.Id == data);

            if(room == null)
            {
                throw new NotFoundException("Room",data);
            }

            room.IsActive = false;

            _ctx.SaveChanges();

            _cache.Remove($"room:{data}");
        }
    }
}
