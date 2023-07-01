using GeoCoordinatePortable;
using UavPathOptimization.Domain.Contracts;

namespace UavPathOptimization.Application.Common.Services;

public sealed class DistanceCalculator : IDistanceCalculator
{
    public double CalculateDistance(GeoCoordinateDto point1, GeoCoordinateDto point2)
    {
        var p1 = new GeoCoordinate(point1.Latitude, point1.Longitude);
        var p2 = new GeoCoordinate(point2.Latitude, point2.Longitude);

        return p1.GetDistanceTo(p2);
    }
}