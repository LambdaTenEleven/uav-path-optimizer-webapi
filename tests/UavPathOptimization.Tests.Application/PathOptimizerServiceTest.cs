using GeoCoordinatePortable;
using UavPathOptimization.Application.UseCases.PathOptimizer.Queries;

namespace UavPathOptimization.Tests.Application;

[TestClass]
public class OptimizePathQueryTests
{
    [TestMethod]
    public async Task OptimizePath_Test4Nodes()
    {
        var expected = new List<GeoCoordinate>
        {
            new(50.021208, 36.343257), //1
            new(50.021129, 36.340071), //2
            new(50.018267, 36.342377), //3
            new(50.016099, 36.342343) //4
        };

        var path = new List<GeoCoordinate>
        {
            new(50.021208, 36.343257), //1
            new(50.016099, 36.342343), //4
            new(50.021129, 36.340071), //2
            new(50.018267, 36.342377) //3
        };

        var query = new OptimizePathQuery(path);
        var handler = new OptimizePathHandler();

        var result = await handler.Handle(query, CancellationToken.None);

        if (result.IsError)
        {
            Assert.Fail($"Results failed: {result.FirstError.Description}");
        }

        // assert
        CollectionAssert.AreEqual(expected, result.Value.ToList());
    }
}