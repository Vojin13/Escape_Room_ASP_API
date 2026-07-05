using Application.DTO.Search;
using Application.Queries.AuditLogs;
using Implementation.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Admin
{
    [Route("api/admin/audit-logs")]
    [ApiController]
    public class AdminAuditLogsController : ControllerBase
    {
        private readonly UseCaseHandler _handler;

        public AdminAuditLogsController(UseCaseHandler handler)
        {
            _handler = handler;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] AuditLogSearchDTO dto,
                                 [FromServices] IAdminGetAuditLogsQuery query)
        {
            var result = _handler.ExecuteQuery(query, dto);
            return Ok(result);
        }
    }
}
