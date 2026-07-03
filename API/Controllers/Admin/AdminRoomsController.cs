using Application.Commands.Rooms;
using Application.DTO.Rooms;
using Application.DTO.Search;
using Application.Queries.Rooms.Admin;
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

        [HttpGet]
        public IActionResult Get([FromQuery] RoomSearchDTO dto,
                                    [FromServices] IAdminGetRoomsQuery query)
        {
            var result = _handler.ExecuteQuery(query, dto);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetRoom(int id, [FromServices] IAdminGetRoomQuery query)
        {
            var result = _handler.ExecuteQuery(query, id);
            return Ok(result);
        }

        [HttpPatch("{id}/toggle-active")]
        public IActionResult ToggleActive(int id, 
                                          [FromServices] IToggleRoomActiveCommand command)
        {
            _handler.ExecuteCommand(command, id);
            return NoContent();
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
        public IActionResult Delete(int id,
                           [FromServices] IDeleteRoomCommand command)
        {
            _handler.ExecuteCommand(command, id);
            return NoContent();
        }
    }
}
