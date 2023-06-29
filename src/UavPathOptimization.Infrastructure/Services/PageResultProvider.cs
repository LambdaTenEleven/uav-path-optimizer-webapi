using ErrorOr;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UavPathOptimization.Domain.Common;
using UavPathOptimization.Domain.Common.Enums;

namespace UavPathOptimization.Infrastructure.Services;

public class PageResultProvider<T>
{
    public async Task<ErrorOr<ResultPage<T>>> GetPagedResult(
        IQueryable<T> query,
        int pageNumber,
        int pageSize,
        string? sortField,
        SortDirection sortDirection,
        CancellationToken cancellationToken)
    {
        // Apply sorting based on sort field and direction
        if (!string.IsNullOrEmpty(sortField))
        {
            var sortExpression = GetSortExpression(sortField);
            query = sortDirection == SortDirection.Descending
                ? query.OrderByDescending(sortExpression)
                : query.OrderBy(sortExpression);
        }

        int totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new ResultPage<T>(
            items,
            totalCount,
            pageNumber,
            pageSize
        );
    }

    private static Expression<Func<T, object>> GetSortExpression(string sortField)
    {
        var parameter = Expression.Parameter(typeof(T));
        var property = Expression.Property(parameter, sortField);
        var conversion = Expression.Convert(property, typeof(object));
        return Expression.Lambda<Func<T, object>>(conversion, parameter);
    }
}