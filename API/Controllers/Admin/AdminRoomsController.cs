using Application.Commands.Rooms;
using Application.DTO.Rooms;
using Implementation.UseCases;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers.Admin
{
    [Route("api/admin/rooms")]
    [ApiController]
    public class AdminRoomsController : ControllerBase
    {
        private readonly UseCaseHandler _handler;

        public AdminRoomsController(UseCaseHandler handler)
        {
            _handler = handler;
        }

        [HttpPost]
        public IActionResult Post([FromForm] CreateRoomDTO dto,
                                  [FromServices] ICreateRoomCommand command)
        {
            _handler.ExecuteCommand(command, dto);
            return StatusCode(201);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateRoomDTO dto,
                                 [FromServices] IUpdateRoomCommand command)
        {
            dto.Id = id;
            _handler.ExecuteCommand(command, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public void Delete(int id,
                           [FromServices] IDeleteRoomCommand command)
        {
            _handler.ExecuteCommand(command, id);
        }
    }
}
