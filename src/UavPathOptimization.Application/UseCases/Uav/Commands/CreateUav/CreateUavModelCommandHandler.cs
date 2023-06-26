using ErrorOr;
using MapsterMapper;
using MediatR;
using UavPathOptimization.Application.Common.Persistence.Uav;
using UavPathOptimization.Domain.Entities;

namespace UavPathOptimization.Application.UseCases.Uav.Commands.CreateUav;

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
        var uav = _mapper.Map<UavModel>(request);

        return await _mediator.Send(new AddUavModelToDbCommand(uav), cancellationToken);
    }
}