using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Common;
using UavPathOptimization.Domain.Entities.UavEntities;
using UavPathOptimization.Domain.Repositories;

namespace UavPathOptimization.Application.UseCases.UavModels.Queries.GetUavModelsPage;

internal sealed class GetUavModelsPageQueryHandler : IRequestHandler<GetUavModelsPageQuery, ErrorOr<ResultPage<UavModel>>>
{
    private readonly IUavModelRepository _uavModelRepository;

    public GetUavModelsPageQueryHandler(IUavModelRepository uavModelRepository)
    {
        _uavModelRepository = uavModelRepository;
    }

    public async Task<ErrorOr<ResultPage<UavModel>>> Handle(GetUavModelsPageQuery request, CancellationToken cancellationToken)
    {
        var uavModels = await _uavModelRepository.GetPageAsync(
            request.PageNumber,
            request.PageSize,
            request.Keyword,
            request.SortField,
            request.SortDirection,
            cancellationToken
        );

        return uavModels;
    }
}