using ErrorOr;
using MapsterMapper;
using MediatR;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Domain.Entities.UavEntities;
using UavPathOptimization.Domain.Repositories;

namespace UavPathOptimization.Application.UseCases.UavModels.Commands.UpdateUavModel;

internal sealed class UpdateUavModelCommandHandler : IRequestHandler<UpdateUavModelCommand, ErrorOr<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUavModelRepository _uavModelRepository;

    private readonly IMapper _mapper;

    public UpdateUavModelCommandHandler(IUnitOfWork unitOfWork, IUavModelRepository uavModelRepository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _uavModelRepository = uavModelRepository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<Unit>> Handle(UpdateUavModelCommand request, CancellationToken cancellationToken)
    {
        var uavModel = await _uavModelRepository.GetByIdAsync(request.Id, cancellationToken);
        if (uavModel == null)
        {
            return Errors.UavModelErrors.UavModelNotFound;
        }

        // if name already exists return error
        var uavByName = await _uavModelRepository.GetByNameAsync(request.Name, cancellationToken);
        if (uavByName != null && uavByName.Id != request.Id)
        {
            return Errors.UavModelErrors.UavModelNameAlreadyExist;
        }

        var uav = _mapper.Map<UavModel>(request);

        _uavModelRepository.Update(uav);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}