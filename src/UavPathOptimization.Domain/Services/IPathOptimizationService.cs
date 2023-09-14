using ErrorOr;
using UavPathOptimization.Domain.Contracts;
using UavPathOptimization.Domain.Contracts.OptimizePath;

namespace UavPathOptimization.Domain.Services;

public interface IPathOptimizationService
{
    ErrorOr<IList<UavPath>> OptimizePath(int vehicleCount, IList<GeoCoordinateDto> coordinates);
}