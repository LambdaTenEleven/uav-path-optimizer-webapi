using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UavPathOptimization.Application.Common.Persistence.Uav;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Infrastructure.Common.EntityFramework;

namespace UavPathOptimization.Infrastructure.Persistence.Uav;

public class UpdateUavModelFromDbCommandHandler : IRequestHandler<UpdateUavModelFromDbCommand, ErrorOr<Unit>>
{
    private readonly ApplicationDbContext _dbContext;

    public UpdateUavModelFromDbCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ErrorOr<Unit>> Handle(UpdateUavModelFromDbCommand request, CancellationToken cancellationToken)
    {
        var uav = await _dbContext.UavModels.AsNoTracking().FirstOrDefaultAsync(u => u.Id == request.UavModel.Id, cancellationToken);

        if (uav is null)
        {
            return Errors.UavModelErrors.UavModelNotFound;
        }

        try
        {
            _dbContext.UavModels.Update(request.UavModel);
            await _dbContext.SaveChangesAsync(cancellationToken);
        } catch (Exception e)
        {
            return Errors.UavModelErrors.UavModelNotUpdated;
        }

        return Unit.Value;
    }
}