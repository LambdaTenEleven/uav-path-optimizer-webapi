using ErrorOr;
using MediatR;
using UavPathOptimization.Application.Common.Persistence.Uav;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Infrastructure.Common.EntityFramework;

namespace UavPathOptimization.Infrastructure.Persistence.Uav;

public class AddUavModelToDbCommandHandler : IRequestHandler<AddUavModelToDbCommand, ErrorOr<Guid>>
{
    private readonly ApplicationDbContext _dbContext;

    public AddUavModelToDbCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ErrorOr<Guid>> Handle(AddUavModelToDbCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.UavModels.AddAsync(request.Uav, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return request.Uav.Id;
        }
        catch (Exception e)
        {
            return Errors.UavModelErrors.UavModelNotCreated;
        }
    }
}