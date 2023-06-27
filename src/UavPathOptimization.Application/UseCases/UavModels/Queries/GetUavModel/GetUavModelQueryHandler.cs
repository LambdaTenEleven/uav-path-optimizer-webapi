using ErrorOr;
using MediatR;
using UavPathOptimization.Application.Common.Persistence.Uav;

namespace UavPathOptimization.Application.UseCases.UavModels.Queries.GetUavModel;

public class GetUavModelQueryHandler : IRequestHandler<GetUavModelQuery, ErrorOr<Domain.Entities.UavModel>>
{
    private readonly IMediator _mediator;

    public GetUavModelQueryHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ErrorOr<Domain.Entities.UavModel>> Handle(GetUavModelQuery request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUavModelFromDbByIdQuery(request.Id), cancellationToken);

        return result;
    }
}