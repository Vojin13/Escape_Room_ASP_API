using Application.Commands.Bookings;
using Application.DTO.Bookings;
using Application.DTO.Search;
using Application.Queries.Bookings.Admin;
using Implementation.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Admin
{
    [Route("api/admin/bookings")]
    [ApiController]
    public class AdminBookingsController : ControllerBase
    {
        private readonly UseCaseHandler _handler;

        public AdminBookingsController(UseCaseHandler handler)
        {
            _handler = handler;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] BookingSearchDTO dto,
                                 [FromServices] IAdminGetBookingsQuery query)
        {
            var result = _handler.ExecuteQuery(query, dto);
            return Ok(result);
        }

        [HttpPatch("{id}/status")]
        public IActionResult UpdateStatus(int id, [FromBody] UpdateBookingStatusDTO dto,
                                          [FromServices] IUpdateBookingStatusCommand command)
        {
            dto.Id = id;
            _handler.ExecuteCommand(command, dto);
            return NoContent();
        }
    }
}
