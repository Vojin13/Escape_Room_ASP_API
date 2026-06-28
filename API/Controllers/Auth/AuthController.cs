using Application.Commands.Auth;
using Application.DTO.Auth;
using Implementation.UseCases;
using Microsoft.AspNetCore.Mvc;

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
    }
}
