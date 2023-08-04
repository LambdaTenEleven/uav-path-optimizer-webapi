using ErrorOr;
using MapsterMapper;
using MediatR;
using UavPathOptimization.Application.Common.Persistence.Uav;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Application.UseCases.UavModels.Commands.UpdateUavModel;

internal sealed class UpdateUavModelCommandHandler : IRequestHandler<UpdateUavModelCommand, ErrorOr<Unit>>
{
    private readonly IMediator _mediator;

    private readonly IMapper _mapper;

    public UpdateUavModelCommandHandler(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<ErrorOr<Unit>> Handle(UpdateUavModelCommand request, CancellationToken cancellationToken)
    {
        var uavByName = await _mediator.Send(new GetUavModelFromDbByNameQuery(request.Name), cancellationToken);

        if (!uavByName.IsError)
        {
            return Errors.UavModelErrors.UavModelNameAlreadyExist;
        }

        var uav = _mapper.Map<UavModel>(request);

        var command = new UpdateUavModelFromDbCommand(uav);

        return await _mediator.Send(command, cancellationToken);
    }
}