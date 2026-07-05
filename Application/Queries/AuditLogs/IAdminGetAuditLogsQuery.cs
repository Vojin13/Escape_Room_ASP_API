using Application.DTO;
using Application.DTO.AuditLogs;
using Application.DTO.Search;

namespace Application.Queries.AuditLogs
{
    public interface IAdminGetAuditLogsQuery : IQuery<AuditLogSearchDTO, PagedResponse<AuditLogDTO>>
    {
    }
}
