using Application.Commands.Timeslots;
using Application.DTO.Search;
using Application.DTO.Timeslot;
using Application.Queries.Timeslots.Admin;
using Implementation.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Admin
{
    [Route("api/admin/timeslots")]
    [ApiController]
    public class AdminTimeslotsController : ControllerBase
    {
        private readonly UseCaseHandler _handler;

        public AdminTimeslotsController(UseCaseHandler handler)
        {
            _handler = handler;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] TimeslotSearchDTO dto,
                                 [FromServices] IAdminGetTimeslotsQuery query)
        {
            var result = _handler.ExecuteQuery(query, dto);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id, [FromServices] IAdminGetTimeslotQuery query)
        {
            var result = _handler.ExecuteQuery(query, id);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateTimeslotDTO dto,
                                  [FromServices] ICreateTimeslotCommand command)
        {
            _handler.ExecuteCommand(command, dto);
            return StatusCode(201);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateTimeslotDTO dto,
                                 [FromServices] IUpdateTimeslotCommand command)
        {
            dto.Id = id;
            _handler.ExecuteCommand(command, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id,
                                    [FromServices] IDeleteTimeslotCommand command)
        {
            _handler.ExecuteCommand(command, id);
            return NoContent();
        }
    }
}
