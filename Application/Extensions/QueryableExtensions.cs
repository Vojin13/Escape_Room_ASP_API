using Application.DTO;
using System.Linq.Expressions;

namespace Application.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereContainsIgnoreCase<T>(
            this IQueryable<T> query,
            Expression<Func<T, string>> selector,
            string value)
        {
            if (string.IsNullOrEmpty(value))
                return query;

            var toLower = Expression.Call(selector.Body, nameof(string.ToLower), null);
            var contains = Expression.Call(toLower, nameof(string.Contains), null, Expression.Constant(value.ToLower()));
            var lambda = Expression.Lambda<Func<T, bool>>(contains, selector.Parameters);

            return query.Where(lambda);
        }

        public static IQueryable<T> WhereContainsIgnoreCaseAny<T>(
            this IQueryable<T> query,
            string value,
            params Expression<Func<T, string>>[] selectors)
        {
            if (string.IsNullOrEmpty(value) || selectors.Length == 0)
                return query;

            var lowerValue = value.ToLower();
            var parameter = Expression.Parameter(typeof(T), "x");

            Expression body = null;

            foreach (var selector in selectors)
            {
                var propertyBody = new ParameterReplacer(selector.Parameters[0], parameter).Visit(selector.Body);
                var toLower = Expression.Call(propertyBody, nameof(string.ToLower), null);
                var contains = Expression.Call(toLower, nameof(string.Contains), null, Expression.Constant(lowerValue));

                body = body == null ? contains : Expression.OrElse(body, contains);
            }

            var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
            return query.Where(lambda);
        }

        private class ParameterReplacer : ExpressionVisitor
        {
            private readonly ParameterExpression _target;
            private readonly ParameterExpression _replacement;

            public ParameterReplacer(ParameterExpression target, ParameterExpression replacement)
            {
                _target = target;
                _replacement = replacement;
            }

            protected override Expression VisitParameter(ParameterExpression node)
                => node == _target ? _replacement : base.VisitParameter(node);
        }

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