using Application.DTO;
using Application.DTO.ErrorLogs;
using Application.DTO.Search;

namespace Application.Queries.ErrorLogs.Admin
{
    public interface IAdminGetErrorLogsQuery : IQuery<ErrorLogSearchDTO, PagedResponse<ErrorLogDTO>>
    {
    }
}
