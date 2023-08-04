using ErrorOr;
using MediatR;
using UavPathOptimization.Application.Common.Persistence.Uav;
using UavPathOptimization.Domain.Common;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Application.UseCases.UavModels.Queries.GetUavModelsPage;

internal sealed class GetUavModelsPageQueryHandler : IRequestHandler<GetUavModelsPageQuery, ErrorOr<ResultPage<UavModel>>>
{
    private readonly IMediator _mediator;

    public GetUavModelsPageQueryHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ErrorOr<ResultPage<UavModel>>> Handle(GetUavModelsPageQuery request, CancellationToken cancellationToken)
    {
        var query = new GetUavModelsFromDbQuery(
            request.PageNumber,
            request.PageSize,
            request.Keyword,
            request.SortField,
            request.SortDirection
        );

        var uavModels = await _mediator.Send(query, cancellationToken);

        return uavModels;
    }
}