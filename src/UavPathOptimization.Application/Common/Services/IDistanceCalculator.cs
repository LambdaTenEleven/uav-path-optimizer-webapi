using UavPathOptimization.Domain.Contracts;
using UnitsNet;

namespace UavPathOptimization.Application.Common.Services;

public interface IDistanceCalculator
{
    Length CalculateDistance(GeoCoordinateDto point1, GeoCoordinateDto point2);
}