using ErrorOr;
using MapsterMapper;
using MediatR;
using UavPathOptimization.Application.Common.Persistence.Uav;

namespace UavPathOptimization.Application.UseCases.UavModel.Commands.CreateUavModel;

public class CreateUavModelCommandHandler : IRequestHandler<CreateUavModelCommand, ErrorOr<Unit>>
{
    private readonly IMediator _mediator;

    private readonly IMapper _mapper;

    public CreateUavModelCommandHandler(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<ErrorOr<Unit>> Handle(CreateUavModelCommand request, CancellationToken cancellationToken)
    {
        var uav = _mapper.Map<Domain.Entities.UavModel>(request);

        return await _mediator.Send(new AddUavModelToDbCommand(uav), cancellationToken);
    }
}