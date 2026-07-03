using Application;
using Application.Commands.Bookings;
using Application.DTO.Bookings;
using Application.Queries.Bookings;
using Implementation.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly UseCaseHandler _handler;

        public BookingsController(UseCaseHandler handler)
        {
            _handler = handler;
        }

        [HttpPost("lock")]
        public IActionResult LockTimeslot([FromBody] LockTimeslotDTO dto,
                                          [FromServices] IApplicationUser user,
                                          [FromServices] ILockTimeslotQuery query)
        {
            dto.UserId = user.Id;
            var result = _handler.ExecuteQuery(query, dto);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateBooking([FromBody] CreateBookingDTO dto,
                                           [FromServices] IApplicationUser user,
                                           [FromServices] ICreateBookingCommand command)
        {
            dto.UserId = user.Id;
            _handler.ExecuteCommand(command, dto);
            return StatusCode(201);
        }
    }
}
