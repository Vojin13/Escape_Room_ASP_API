using Application.DTO;

namespace Application.Extensions
{
    public static class QueryableExtensions
    {
        public static PagedResponse<TDto> ToPagedResponse<TDto>(
            this IQueryable<TDto> query,
            int? page,
            int? perPage) where TDto : class
        {
            int currentPage = page ?? 1;
            int currentPerPage = perPage ?? 9;

            int totalCount = query.Count();

            var items = query
                .Skip((currentPage - 1) * currentPerPage)
                .Take(currentPerPage)
                .ToList();

            return new PagedResponse<TDto>
            {
                TotalCount = totalCount,
                CurrentPage = currentPage,
                PerPage = currentPerPage,
                Items = items
            };
        }
    }
}