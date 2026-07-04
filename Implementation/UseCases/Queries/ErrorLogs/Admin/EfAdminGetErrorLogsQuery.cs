using Application.DTO;
using Application.DTO.ErrorLogs;
using Application.DTO.Search;
using Application.Extensions;
using Application.Queries.ErrorLogs.Admin;

namespace Implementation.UseCases.Queries.ErrorLogs.Admin
{
    public class EfAdminGetErrorLogsQuery : EfUseCase, IAdminGetErrorLogsQuery
    {
        public EfAdminGetErrorLogsQuery(AppDbContext context) : base(context)
        {
        }

        public string Name => "Admin Get Error Logs";

        public string Id => "admin-get-error-logs";

        public PagedResponse<ErrorLogDTO> Execute(ErrorLogSearchDTO request)
        {
            var query = _ctx.ErrorLogs.AsQueryable();

            if(!string.IsNullOrEmpty(request.Code))
            {
                query = query.Where(x => x.SupportCode.ToString().Contains(request.Code));
            }

            query = query.WhereContainsIgnoreCaseAny(request.Keyword, x => x.User.Username, x => x.User.Email);

            query = request.SortDescending
                ? query.OrderByDescending(x => x.CreatedAt)
                : query.OrderBy(x => x.CreatedAt);

            return query.Select(x => new ErrorLogDTO
            {
                Id = x.Id,
                SupportCode = x.SupportCode,
                Message = x.Message,
                File = x.File,
                Line = x.Line,
                Url = x.Url,
                Method = x.Method,
                Payload = x.Payload,
                UserId = x.UserId,
                Username = x.User != null ? x.User.Username : null,
                CreatedAt = x.CreatedAt
            }).ToPagedResponse(request.Page, request.PerPage);
        }
    }
}
