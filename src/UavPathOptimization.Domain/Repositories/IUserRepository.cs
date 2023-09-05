using UavPathOptimization.Domain.Common;
using UavPathOptimization.Domain.Common.Enums;
using UavPathOptimization.Domain.Entities;

namespace UavPathOptimization.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<ResultPage<User>> GetPageAsync(
        int pageNumber,
        int pageSize,
        string? sortField,
        SortDirection sortDirection,
        CancellationToken cancellationToken);
    void Add(User entity);
    void UpdateAsync(User entity);
    void Delete(User entity);
}