using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Common;
using UavPathOptimization.Domain.Common.Enums;
using UavPathOptimization.Domain.Entities;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Application.Common.Persistence.Uav;

public record GetUavModelsFromDbQuery(
    int PageNumber,
    int PageSize,
    string? Keyword,
    string? SortField,
    SortDirection SortDirection
) : IRequest<ErrorOr<ResultPage<UavModel>>>;