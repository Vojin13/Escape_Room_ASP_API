using Application;
using Application.Queries.Users;
using Implementation.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UseCaseHandler _handler;

        public UsersController(UseCaseHandler handler)
        {
            _handler = handler;
        }

        [HttpGet("me")]
        public IActionResult GetMyProfile([FromServices] IGetMyProfileQuery query,
                                          [FromServices] IApplicationUser user)
        {
            var result = _handler.ExecuteQuery(query, user.Id);
            return Ok(result);
        }
    }
}
