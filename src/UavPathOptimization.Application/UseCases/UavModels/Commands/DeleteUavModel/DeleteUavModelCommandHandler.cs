using ErrorOr;
using MediatR;
using UavPathOptimization.Application.Common.Persistence.Uav;

namespace UavPathOptimization.Application.UseCases.UavModels.Commands.DeleteUavModel;

public class DeleteUavModelCommandHandler : IRequestHandler<DeleteUavModelCommand, ErrorOr<Unit>>
{
    private readonly IMediator _mediator;

    public DeleteUavModelCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ErrorOr<Unit>> Handle(DeleteUavModelCommand request, CancellationToken cancellationToken)
    {
        var command = new DeleteUavModelFromDbCommand(request.id);

        return await _mediator.Send(command, cancellationToken);
    }
}