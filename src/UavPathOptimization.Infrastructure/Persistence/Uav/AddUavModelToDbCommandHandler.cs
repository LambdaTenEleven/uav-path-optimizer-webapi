using ErrorOr;
using MediatR;
using UavPathOptimization.Application.Common.Persistence.Uav;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Domain.Entities.UavEntities;
using UavPathOptimization.Infrastructure.Common.EntityFramework;

namespace UavPathOptimization.Infrastructure.Persistence.Uav;

public sealed class AddUavModelToDbCommandHandler : IRequestHandler<AddUavModelToDbCommand, ErrorOr<UavModel>>
{
    private readonly ApplicationDbContext _dbContext;

    public AddUavModelToDbCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ErrorOr<UavModel>> Handle(AddUavModelToDbCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.UavModels.AddAsync(request.Uav, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return request.Uav;
        }
        catch (Exception e)
        {
            return Errors.UavModelErrors.UavModelNotCreated;
        }
    }
}