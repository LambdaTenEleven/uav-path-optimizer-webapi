using UavPathOptimization.Domain.Common;
using UavPathOptimization.Domain.Common.Enums;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Domain.Repositories;

public interface IUavModelRepository
{
    Task<UavModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UavModel?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<ResultPage<UavModel>> GetPageAsync(
        int pageNumber,
        int pageSize,
        string? keyword,
        string? sortField,
        SortDirection sortDirection,
        CancellationToken cancellationToken);
    void Add(UavModel entity);
    void Update(UavModel entity);
    void Delete(UavModel entity);
}