using NUnit.Framework;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Domain.Contracts;
using UavPathOptimization.Domain.Services;

namespace UavPathOptimization.Domain.UnitTests.Services;

public class PathOptimizationServiceTests
{
    private PathOptimizationService _pathOptimizationService;

    [SetUp]
    public void Setup()
    {
        _pathOptimizationService = new PathOptimizationService();
    }

    [Test]
    public void OptimizePath_When_CoordinatesAreNull_Returns_Error()
    {
        var result = _pathOptimizationService.OptimizePath(1, null);

        Assert.IsTrue(result.IsError);
        Assert.That(result.FirstError, Is.EqualTo(Errors.OptimizePath.InvalidInputError));
    }

    [Test]
    public void OptimizePath_When_CoordinatesCountIsLessThanTwo_Returns_Error()
    {
        var coordinates = new List<GeoCoordinateDto>
        {
            new(42.3616697, -71.07873001)
        };

        var result = _pathOptimizationService.OptimizePath(1, coordinates);

        Assert.IsTrue(result.IsError);
        Assert.That(result.FirstError, Is.EqualTo(Errors.OptimizePath.InvalidInputError));
    }

    [Test]
    public void OptimizePath_When_ValidInput_Returns_ValidUavPaths()
    {
        var coordinates = new List<GeoCoordinateDto>
        {
            new(42.3616697, -71.07873001),
            new(42.3616287, -71.07864984),
            new(42.3615267, -71.07879001),
            new(42.3615621, -71.0787321)
        };

        int vehicleCount = 2;

        var result = _pathOptimizationService.OptimizePath(vehicleCount, coordinates);

        Assert.IsFalse(result.IsError);
        Assert.IsNotNull(result.Value);
        Assert.That(result.Value.Count, Is.EqualTo(vehicleCount));

        // Check if all original points are visited in the solution
        var visitedPoints = new HashSet<GeoCoordinateDto>();
        foreach (var uavPath in result.Value)
        {
            foreach (var point in uavPath.Path)
            {
                visitedPoints.Add(point);
            }
        }

        Assert.That(visitedPoints.Count, Is.EqualTo(coordinates.Count));

        // Check if the same path is not assigned to multiple vehicles
        var assignedPaths = new HashSet<int>();
        foreach (var uavPath in result.Value)
        {
            assignedPaths.Add(uavPath.Path.GetHashCode());
        }

        Assert.That(assignedPaths.Count, Is.EqualTo(result.Value.Count));
    }
}