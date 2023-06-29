using ErrorOr;
using MediatR;
using UavPathOptimization.Application.Common.Persistence.Uav;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Infrastructure.Common.EntityFramework;

namespace UavPathOptimization.Infrastructure.Persistence.Uav;

public class DeleteUavModelFromDbCommandHandler : IRequestHandler<DeleteUavModelFromDbCommand, ErrorOr<Unit>>
{
    private readonly ApplicationDbContext _dbContext;

    public DeleteUavModelFromDbCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ErrorOr<Unit>> Handle(DeleteUavModelFromDbCommand request, CancellationToken cancellationToken)
    {
        var uavModel = _dbContext.UavModels.FirstOrDefault(x => x.Id == request.Id);
        if (uavModel is null)
        {
            return Errors.UavModelErrors.UavModelNotFound;
        }

        try
        {
            _dbContext.UavModels.Remove(uavModel);
            await _dbContext.SaveChangesAsync(cancellationToken);
        } catch (Exception e)
        {
            return Errors.UavModelErrors.UavModelDeletionFailed;
        }

        return Unit.Value;
    }
}