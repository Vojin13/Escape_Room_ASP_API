using Application.DTO.Search;
using Application.Queries.ErrorLogs.Admin;
using Implementation.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Admin
{
    [Route("api/admin/error-logs")]
    [ApiController]
    public class AdminErrorLogsController : ControllerBase
    {
        private readonly UseCaseHandler _handler;

        public AdminErrorLogsController(UseCaseHandler handler)
        {
            _handler = handler;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] ErrorLogSearchDTO dto,
                                 [FromServices] IAdminGetErrorLogsQuery query)
        {
            var result = _handler.ExecuteQuery(query, dto);
            return Ok(result);
        }
    }
}
