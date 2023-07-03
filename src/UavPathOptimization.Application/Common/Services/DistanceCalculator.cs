using GeoCoordinatePortable;
using UavPathOptimization.Domain.Contracts;
using UnitsNet;
using UnitsNet.Units;

namespace UavPathOptimization.Application.Common.Services;

public sealed class DistanceCalculator : IDistanceCalculator
{
    public Length CalculateDistance(GeoCoordinateDto point1, GeoCoordinateDto point2)
    {
        var p1 = new GeoCoordinate(point1.Latitude, point1.Longitude);
        var p2 = new GeoCoordinate(point2.Latitude, point2.Longitude);

        return new Length(p1.GetDistanceTo(p2), LengthUnit.Meter);
    }
}