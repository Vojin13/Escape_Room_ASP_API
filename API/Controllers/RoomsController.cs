using Application.DTO.Rooms;
using Application.DTO.Search;
using Application.Queries.Rooms;
using Implementation.UseCases;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly UseCaseHandler _handler;

        public RoomsController(UseCaseHandler handler)
        {
            _handler = handler;
        }

        // GET: api/<RoomsController>
        [HttpGet]
        public IActionResult Get([FromQuery] RoomSearchDTO dto,
                                 [FromServices] IGetRoomsQuery query)
        {
            var result = _handler.ExecuteQuery(query, dto);
            return Ok(result);
        }

        // GET api/<RoomsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id,
                                 [FromServices] IGetRoomQuery query)
        {
            var result = _handler.ExecuteQuery(query, id);
            return Ok(result);
        }

        [HttpGet("{id}/availability")]
        public IActionResult GetAvailability(int id, [FromQuery] DateTime date,
                                     [FromServices] IGetRoomAvailabilityQuery query)
        {
            var result = _handler.ExecuteQuery(query, new RoomAvailabilityDTO
            {
                RoomId = id,
                Date = date
            });
            return Ok(result);
        }
    }
}
