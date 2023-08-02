namespace UavPathOptimization.Domain.Contracts;

public record UavPathDto(Guid UavModelId, IList<GeoCoordinateDto> Coordinates);