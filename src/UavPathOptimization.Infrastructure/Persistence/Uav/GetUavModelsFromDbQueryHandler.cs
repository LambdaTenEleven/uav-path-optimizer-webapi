using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UavPathOptimization.Application.Common.Persistence.Uav;
using UavPathOptimization.Domain.Common;
using UavPathOptimization.Domain.Entities;
using UavPathOptimization.Infrastructure.Common.EntityFramework;
using UavPathOptimization.Infrastructure.Services;

namespace UavPathOptimization.Infrastructure.Persistence.Uav;

public class GetUavModelsFromDbQueryHandler : IRequestHandler<GetUavModelsFromDbQuery, ErrorOr<ResultPage<UavModel>>>
{
    private readonly ApplicationDbContext _dbContext;

    private readonly PageResultProvider<UavModel> _resultProvider;

    public GetUavModelsFromDbQueryHandler(ApplicationDbContext dbContext, PageResultProvider<UavModel> pageResultProvider)
    {
        _dbContext = dbContext;
        _resultProvider = pageResultProvider;
    }

    public async Task<ErrorOr<ResultPage<UavModel>>> Handle(GetUavModelsFromDbQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.UavModels.AsQueryable();

        // Apply search filter if keyword is provided
        if (!string.IsNullOrEmpty(request.Keyword))
        {
            query = query.Where(uavModel =>
                uavModel.Name.Contains(request.Keyword));
        }

        return await _resultProvider.GetPagedResult(query, request.PageNumber, request.PageSize, request.SortField, request.SortDirection, cancellationToken);
    }
}