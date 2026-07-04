using Application;
using Application.Commands.Bookings;
using Application.DTO;
using Application.DTO.Bookings;
using Application.DTO.Search;
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

        [HttpPatch("cancel")]
        public IActionResult CancelBooking([FromBody] CancelBookingDTO dto,
                                            [FromServices] IApplicationUser user,
                                            [FromServices] ICancelBookingCommand command)
        {
            dto.UserId = user.Id;
            _handler.ExecuteCommand(command, dto);
            return NoContent();
        }

        [HttpGet("statuses")]
        public IActionResult GetStatuses([FromServices] IGetBookingStatusesQuery query)
        {
            var result = _handler.ExecuteQuery(query, NoData.Instance);
            return Ok(result);
        }

        [HttpGet("my-bookings")]
        public IActionResult GetMyBookings([FromQuery] MyBookingSearchDTO dto,
                                           [FromServices] IApplicationUser user,
                                           [FromServices] IGetMyBookingsQuery query)
        {
            dto.UserId = user.Id;
            var result = _handler.ExecuteQuery(query, dto);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetBooking(int id,
                                        [FromQuery] GetBookingDTO dto,
                                        [FromServices] IApplicationUser user,
                                        [FromServices] IGetBookingQuery query)
        {
            dto.UserId = user.Id;
            dto.BookingId = id;
            var result = _handler.ExecuteQuery(query, dto);
            return Ok(result);
        }
    }
}
