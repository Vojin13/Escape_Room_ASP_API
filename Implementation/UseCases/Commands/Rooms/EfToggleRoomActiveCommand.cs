using Application.Commands.Rooms;
using Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implementation.UseCases.Commands.Rooms
{
    public class EfToggleRoomActiveCommand : EfUseCase, IToggleRoomActiveCommand
    {
        public EfToggleRoomActiveCommand(AppDbContext context) : base(context)
        {
        }

        public string Name => "Toggle Room Activation";

        public string Id => "toggle-room-active";

        public void Execute(int data)
        {
            var room = _ctx.Rooms.FirstOrDefault(r => r.Id == data);

            if(room == null)
            {
                throw new NotFoundException("Room", data);
            }

            room.IsActive = !room.IsActive;
            _ctx.SaveChanges();
        }
    }
}
