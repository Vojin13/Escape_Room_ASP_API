using Application;
using Application.Commands.Reviews;
using Application.DTO.Reviews;
using Implementation.UseCases;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly UseCaseHandler _handler;

        public ReviewsController(UseCaseHandler handler)
        {
            _handler = handler;
        }


        // POST api/<ReviewsController>
        [HttpPost]
        public IActionResult Post([FromBody] CreateReviewDTO dto,
                                  [FromServices] ICreateReviewCommand command,
                                  [FromServices] IApplicationUser user)
        {
            dto.UserId = user.Id;
            _handler.ExecuteCommand(command, dto);
            return StatusCode(201);
        }

        // DELETE api/<ReviewsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id,
                                    [FromServices] IDeleteReviewCommand command)
        {
            _handler.ExecuteCommand(command, id);
            return NoContent();
        }
    }
}
