using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UavPathOptimization.Domain.Common.Enums;
using UavPathOptimization.Domain.Entities.Base;
using UavPathOptimization.Infrastructure.Common.EntityFramework;

namespace UavPathOptimization.Infrastructure.Persistence;

internal abstract class Repository<TEntity> where TEntity : Entity
{
    protected readonly ApplicationDbContext DbContext;

    protected Repository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<TEntity>().AsNoTracking().SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public virtual void Add(TEntity entity)
    {
        DbContext.Set<TEntity>().Add(entity);
    }

    public virtual void Update(TEntity entity)
    {
        DbContext.Set<TEntity>().Update(entity);
    }

    public virtual void Delete(TEntity entity)
    {
        DbContext.Set<TEntity>().Remove(entity);
    }

    protected static IQueryable<TEntity> FormPageQuery(
        IQueryable<TEntity> query,
        int pageNumber,
        int pageSize,
        string? sortField,
        SortDirection sortDirection)
    {
        if (!string.IsNullOrEmpty(sortField))
        {
            var sortExpression = GetSortExpression(sortField);
            query = sortDirection == SortDirection.Descending
                ? query.OrderByDescending(sortExpression)
                : query.OrderBy(sortExpression);
        }

        var items = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        return items;
    }

    private static Expression<Func<TEntity, object>> GetSortExpression(string sortField)
    {
        var parameter = Expression.Parameter(typeof(TEntity));
        var property = Expression.Property(parameter, sortField);
        var conversion = Expression.Convert(property, typeof(object));
        return Expression.Lambda<Func<TEntity, object>>(conversion, parameter);
    }
}