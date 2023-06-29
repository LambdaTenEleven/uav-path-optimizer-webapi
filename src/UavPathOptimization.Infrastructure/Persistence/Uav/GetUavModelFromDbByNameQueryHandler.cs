using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UavPathOptimization.Application.Common.Persistence.Uav;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Domain.Entities.UavEntities;
using UavPathOptimization.Infrastructure.Common.EntityFramework;

namespace UavPathOptimization.Infrastructure.Persistence.Uav;

public class GetUavModelFromDbByNameQueryHandler : IRequestHandler<GetUavModelFromDbByNameQuery, ErrorOr<UavModel>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetUavModelFromDbByNameQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ErrorOr<UavModel>> Handle(GetUavModelFromDbByNameQuery request, CancellationToken cancellationToken)
    {
        var uavModel = await _dbContext.UavModels
            .FirstOrDefaultAsync(u => u.Name == request.Name, cancellationToken);

        if (uavModel is null)
        {
            return Errors.UavModelErrors.UavModelNotFound;
        }

        return uavModel;
    }
}