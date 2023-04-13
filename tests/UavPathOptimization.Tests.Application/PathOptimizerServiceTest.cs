using GeoCoordinatePortable;
using UavPathOptimization.Application.Services;

namespace UavPathOptimization.Tests.Application;

[TestClass]
public class PathOptimizerServiceTest
{
    [TestMethod]
    public void OptimizePath_Test4Nodes()
    {
        var expected = new List<GeoCoordinate>
        {
            new (50.021208, 36.343257), //1
            new (50.021129, 36.340071), //2
            new (50.018267, 36.342377), //3
            new (50.016099, 36.342343), //4
        };

        var path = new List<GeoCoordinate>
        {
            new (50.021208, 36.343257), //1
            new (50.016099, 36.342343), //4
            new (50.021129, 36.340071), //2
            new (50.018267, 36.342377), //3
        };

        var service = new PathOptimizerService();
        var actual = service.OptimizePath(path).ToList();

        // assert
        CollectionAssert.AreEqual(expected, actual);
    }
}