using Microsoft.EntityFrameworkCore;
using UavPathOptimization.Domain.Common;
using UavPathOptimization.Domain.Common.Enums;
using UavPathOptimization.Domain.Entities.UavEntities;
using UavPathOptimization.Domain.Repositories;
using UavPathOptimization.Infrastructure.Common.EntityFramework;

namespace UavPathOptimization.Infrastructure.Persistence.Uav;

internal sealed class UavModelRepository : Repository<UavModel>, IUavModelRepository
{
    public UavModelRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<UavModel?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbContext.UavModels.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }

    public async Task<ResultPage<UavModel>> GetPageAsync(int pageNumber, int pageSize, string? keyword, string? sortField, SortDirection sortDirection,
        CancellationToken cancellationToken)
    {
        var query = DbContext.UavModels.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(x => x.Name.Contains(keyword));
        }

        int totalCount = await query.CountAsync(cancellationToken);

        query = FormPageQuery(query, pageNumber, pageSize, sortField, sortDirection);
        var items = await query.ToListAsync(cancellationToken);

        return new ResultPage<UavModel>(items, totalCount, pageNumber, pageSize);
    }
}