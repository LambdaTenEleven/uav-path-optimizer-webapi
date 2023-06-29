using ErrorOr;
using MediatR;
using UavPathOptimization.Application.Common.Persistence.Uav;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Application.UseCases.UavModels.Commands.UpdateUavModel;

public class UpdateUavModelCommandHandler : IRequestHandler<UpdateUavModelCommand, ErrorOr<Unit>>
{
    private readonly IMediator _mediator;

    public UpdateUavModelCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ErrorOr<Unit>> Handle(UpdateUavModelCommand request, CancellationToken cancellationToken)
    {
        var command = new UpdateUavModelFromDbCommand(
            new UavModel(
                request.Id,
                request.Name,
                request.MaxSpeed,
                request.MaxFlightTime
            )
        );

        return await _mediator.Send(command, cancellationToken);
    }
}