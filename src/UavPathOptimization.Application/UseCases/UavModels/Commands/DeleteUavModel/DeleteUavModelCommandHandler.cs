using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Domain.Repositories;

namespace UavPathOptimization.Application.UseCases.UavModels.Commands.DeleteUavModel;

internal sealed class DeleteUavModelCommandHandler : IRequestHandler<DeleteUavModelCommand, ErrorOr<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUavModelRepository _uavModelRepository;

    public DeleteUavModelCommandHandler(IUnitOfWork unitOfWork, IUavModelRepository uavModelRepository)
    {
        _unitOfWork = unitOfWork;
        _uavModelRepository = uavModelRepository;
    }

    public async Task<ErrorOr<Unit>> Handle(DeleteUavModelCommand request, CancellationToken cancellationToken)
    {
        var uavModel = await _uavModelRepository.GetByIdAsync(request.Id, cancellationToken);
        if (uavModel is null)
        {
            return Errors.UavModelErrors.UavModelNotFound;
        }

        _uavModelRepository.Delete(uavModel);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}