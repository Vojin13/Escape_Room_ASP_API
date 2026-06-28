using Application.Commands;
using Application.DTO;
using Implementation.UseCases;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly UseCaseHandler _handler;

        public SeedController(UseCaseHandler handler)
        {
            _handler = handler;
        }

        [HttpPost]
        public IActionResult Seed([FromServices] ISeedCommand command)
        {
            _handler.ExecuteCommand(command, NoData.Instance);
            return Ok("Seed completed.");
        }
    }
}
