using Application.Commands.Auth;
using Application.DTO.Auth;
using Application.Queries.Auth;
using Implementation.UseCases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UseCaseHandler _handler;

        public AuthController(UseCaseHandler handler)
        {
            _handler = handler;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserDTO dto,
                                      [FromServices] IRegisterUserCommand command)
        {
            _handler.ExecuteCommand(command, dto);
            return StatusCode(201);
        }

        [EnableRateLimiting("login")]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO dto,
                                   [FromServices] ILoginUserQuery query)
        {
            var result = _handler.ExecuteQuery(query, dto);
            return Ok(result);
        }

        [HttpPost("logout")]
        public IActionResult Logout([FromBody] LogoutDTO dto,
                            [FromServices] ILogoutUserCommand command)
        {
            _handler.ExecuteCommand(command, dto);
            return NoContent();
        }

        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] RefreshTokenDTO dto,
                             [FromServices] IRefreshTokenQuery query)
        {
            var result = _handler.ExecuteQuery(query, dto);
            return Ok(result);
        }

        [HttpPost("activate")]
        public IActionResult Activate([FromQuery] string code,
                             [FromServices] IActivateAccountCommand command)
        {
            _handler.ExecuteCommand(command, code);
            return NoContent();
        }
    }
}
