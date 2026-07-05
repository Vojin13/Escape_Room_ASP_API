using Application.DTO;
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
        public IActionResult GetMyProfile([FromServices] IGetMyProfileQuery query)
        {
            var result = _handler.ExecuteQuery(query, NoData.Instance);
            return Ok(result);
        }
    }
}
