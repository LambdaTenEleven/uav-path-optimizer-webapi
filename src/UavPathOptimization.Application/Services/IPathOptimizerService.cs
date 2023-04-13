using GeoCoordinatePortable;

namespace UavPathOptimization.Application.Services;

public interface IPathOptimizerService
{
    public IList<GeoCoordinate> OptimizePath(IList<GeoCoordinate> path);
}