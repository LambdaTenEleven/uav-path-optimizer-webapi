using UavPathOptimization.Domain.Common.Enums;

namespace UavPathOptimization.Domain.Contracts.UavModel;

public record GetUavModelsPageRequest(
    int Page,
    int Size,
    string? Keyword = null,
    string? SortField = null,
    SortDirection SortDirection = SortDirection.Ascending
);
