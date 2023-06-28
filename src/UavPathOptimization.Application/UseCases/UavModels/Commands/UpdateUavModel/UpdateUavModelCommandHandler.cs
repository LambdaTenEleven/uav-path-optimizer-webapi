using ErrorOr;
using MediatR;
using UavPathOptimization.Application.Common.Persistence.Uav;

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
        var command = new UpdateUavModelFromDbCommand(request.UavModel);

        return await _mediator.Send(command, cancellationToken);
    }
}