using UavPathOptimization.Domain.Repositories;
using UavPathOptimization.Infrastructure.Common.EntityFramework;

namespace UavPathOptimization.Infrastructure.Persistence;

internal sealed class UnitOfWork : IUnitOfWork
{
    private ApplicationDbContext _dbContext;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}