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
            int currentPerPage = perPage ?? 10;

            return new PagedResponse<TDto>
            {
                TotalCount = query.Count(),
                CurrentPage = currentPage,
                PerPage = currentPerPage,
                Items = query
                    .Skip((currentPage - 1) * currentPerPage)
                    .Take(currentPerPage)
                    .ToList()
            };
        }
    }

}
