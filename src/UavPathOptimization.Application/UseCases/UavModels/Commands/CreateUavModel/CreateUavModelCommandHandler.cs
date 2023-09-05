using ErrorOr;
using MapsterMapper;
using MediatR;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Domain.Entities.UavEntities;
using UavPathOptimization.Domain.Repositories;

namespace UavPathOptimization.Application.UseCases.UavModels.Commands.CreateUavModel;

internal sealed class CreateUavModelCommandHandler : IRequestHandler<CreateUavModelCommand, ErrorOr<UavModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUavModelRepository _uavModelRepository;

    private readonly IMapper _mapper;

    public CreateUavModelCommandHandler(IUnitOfWork unitOfWork, IUavModelRepository uavModelRepository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _uavModelRepository = uavModelRepository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<UavModel>> Handle(CreateUavModelCommand request, CancellationToken cancellationToken)
    {
        var uavModel = await _uavModelRepository.GetByNameAsync(request.Name, cancellationToken);
        if (uavModel is not null)
        {
            return Errors.UavModelErrors.UavModelNameAlreadyExist;
        }

        var uav = _mapper.Map<UavModel>(request);

        _uavModelRepository.Add(uav);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return uav;
    }
}