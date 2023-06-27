using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UavPathOptimization.Application.Common.Persistence.Uav;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Domain.Entities;
using UavPathOptimization.Infrastructure.Common.EntityFramework;

namespace UavPathOptimization.Infrastructure.Persistence.Uav;

public class GetUavModelFromDbQueryHandler : IRequestHandler<GetUavModelFromDbByIdQuery, ErrorOr<UavModel>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetUavModelFromDbQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ErrorOr<UavModel>> Handle(GetUavModelFromDbByIdQuery request, CancellationToken cancellationToken)
    {
        var uav = await _dbContext.UavModels.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (uav is null)
        {
            return Errors.UavModelErrors.UavModelNotFound;
        }

        return uav;
    }
}