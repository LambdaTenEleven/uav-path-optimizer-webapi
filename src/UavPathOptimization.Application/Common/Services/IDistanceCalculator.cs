using UavPathOptimization.Domain.Contracts;

namespace UavPathOptimization.Application.Common.Services;

public interface IDistanceCalculator
{
    double CalculateDistance(GeoCoordinateDto point1, GeoCoordinateDto point2);
}