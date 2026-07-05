using Application.DTO;
using Application.DTO.AuditLogs;
using Application.DTO.Search;
using Application.Extensions;
using Application.Queries.AuditLogs;

namespace Implementation.UseCases.Queries.AuditLogs
{
    public class EfAdminGetAuditLogsQuery : EfUseCase, IAdminGetAuditLogsQuery
    {
        public EfAdminGetAuditLogsQuery(AppDbContext context) : base(context)
        {
        }

        public string Name => "Admin Get Audit Logs";

        public string Id => "admin-get-audit-logs";

        public PagedResponse<AuditLogDTO> Execute(AuditLogSearchDTO request)
        {
            var query = _ctx.AuditLogs.AsQueryable();

            query = query.WhereContainsIgnoreCaseAny(request.Keyword,
                x => x.User.Username, x => x.User.Email, x => x.UseCaseName, x => x.UseCaseId);

            if (!string.IsNullOrEmpty(request.Method))
            {
                query = query.Where(x => x.Method == request.Method.ToUpper());
            }

            if (request.DateFrom.HasValue)
            {
                var from = DateTime.SpecifyKind(request.DateFrom.Value.Date, DateTimeKind.Utc);
                query = query.Where(x => x.CreatedAt >= from);
            }

            if (request.DateTo.HasValue)
            {
                var to = DateTime.SpecifyKind(request.DateTo.Value.Date.AddDays(1), DateTimeKind.Utc);
                query = query.Where(x => x.CreatedAt < to);
            }

            query = request.SortDescending
                ? query.OrderByDescending(x => x.CreatedAt)
                : query.OrderBy(x => x.CreatedAt);

            return query.Select(x => new AuditLogDTO
            {
                Id = x.Id,
                UserId = x.UserId,
                Username = x.User != null ? x.User.Username : null,
                UseCaseId = x.UseCaseId,
                UseCaseName = x.UseCaseName,
                WasAuthorized = x.WasAuthorized,
                Method = x.Method,
                ElapsedMs = x.ElapsedMs,
                CreatedAt = x.CreatedAt
            }).ToPagedResponse(request.Page, request.PerPage);
        }
    }
}
