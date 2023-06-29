using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Common;
using UavPathOptimization.Domain.Common.Enums;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Application.UseCases.UavModels.Queries.GetUavModelsPage;

public record GetUavModelsPageQuery(
    int PageNumber,
    int PageSize,
    string? Keyword = null,
    string? SortField = null,
    SortDirection SortDirection = SortDirection.Ascending
) : IRequest<ErrorOr<ResultPage<UavModel>>>;