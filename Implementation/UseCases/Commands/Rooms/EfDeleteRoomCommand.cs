using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Commands.Rooms;
using Application.Exceptions;

namespace Implementation.UseCases.Commands.Rooms
{
    public class EfDeleteRoomCommand : EfUseCase, IDeleteRoomCommand
    {
        public EfDeleteRoomCommand(AppDbContext context) : base(context)
        {
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
        }
    }
}
