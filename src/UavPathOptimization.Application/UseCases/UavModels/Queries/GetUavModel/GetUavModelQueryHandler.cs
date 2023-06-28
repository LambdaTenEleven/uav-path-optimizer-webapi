using ErrorOr;
using MediatR;
using UavPathOptimization.Application.Common.Persistence.Uav;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Application.UseCases.UavModels.Queries.GetUavModel;

public class GetUavModelQueryHandler : IRequestHandler<GetUavModelQuery, ErrorOr<UavModel>>
{
    private readonly IMediator _mediator;

    public GetUavModelQueryHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ErrorOr<UavModel>> Handle(GetUavModelQuery request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUavModelFromDbByIdQuery(request.Id), cancellationToken);

        return result;
    }
}