using ErrorOr;
using GeoCoordinatePortable;
using Google.OrTools.ConstraintSolver;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Domain.Contracts;
using UavPathOptimization.Domain.Contracts.OptimizePath;

namespace UavPathOptimization.Domain.Services;

public class PathOptimizationService : IPathOptimizationService
{
    private const int Scale = 100;

    public ErrorOr<IList<UavPath>> OptimizePath(int vehicleCount, IList<GeoCoordinateDto> coordinates)
    {
        if (coordinates == null || coordinates.Count < 2)
        {
            return Errors.OptimizePath.InvalidInputError;
        }

        var path = coordinates.Select(x => new GeoCoordinate(x.Latitude, x.Longitude)).ToList();

        var matrix = CreateDistanceMatrix(path);

        var (routing, manager) = InitializeRoutingModel(matrix, vehicleCount);

        var searchParameters = ConfigureRoutingSearchParameters();

        var solution = routing.SolveWithParameters(searchParameters);
        if (solution is null)
        {
            return Errors.OptimizePath.SolutionError;
        }

        var result = ExtractUavPaths(solution, manager, routing, vehicleCount, coordinates).ToList();
        return result;
    }

    private static long[,] CreateDistanceMatrix(IEnumerable<GeoCoordinate> path)
    {
        var coordinatesList = path.ToList();
        var count = coordinatesList.Count;
        var matrix = new long[count, count];

        for (var i = 0; i < count; i++)
        {
            for (var j = i; j < count; j++)
            {
                var distance = (long)(coordinatesList[i].GetDistanceTo(coordinatesList[j]) * Scale);
                matrix[i, j] = distance;
                matrix[j, i] = distance;
            }
        }

        return matrix;
    }

    private static (RoutingModel, RoutingIndexManager) InitializeRoutingModel(long[,] matrix, int vehicleCount)
    {
        var manager = new RoutingIndexManager(matrix.GetLength(0), vehicleCount, 0);
        var routing = new RoutingModel(manager);

        int transitCallbackIndex = routing.RegisterTransitCallback((long fromIndex, long toIndex) =>
        {
            // Convert from routing variable Index to
            // distance matrix NodeIndex.
            var fromNode = manager.IndexToNode(fromIndex);
            var toNode = manager.IndexToNode(toIndex);
            return matrix[fromNode, toNode];
        });

        routing.SetArcCostEvaluatorOfAllVehicles(transitCallbackIndex);
        routing.AddDimension(transitCallbackIndex, 0, 30000000,
            true, // start cumul to zero
            "Distance");
        routing.GetMutableDimension("Distance").SetGlobalSpanCostCoefficient(100000);

        return (routing, manager);
    }

    private static RoutingSearchParameters ConfigureRoutingSearchParameters()
    {
        var searchParameters = operations_research_constraint_solver.DefaultRoutingSearchParameters();
        searchParameters.FirstSolutionStrategy = FirstSolutionStrategy.Types.Value.PathCheapestArc;
        return searchParameters;
    }

    private static IList<UavPath> ExtractUavPaths(Assignment solution, RoutingIndexManager manager,
        RoutingModel routing, int vehicleCount, IList<GeoCoordinateDto> coordinates)
    {
        var uavPaths = new List<UavPath>();

        for (int i = 0; i < vehicleCount; ++i)
        {
            long routeDistance = 0;
            var index = routing.Start(i);

            var uavPath = new UavPath();
            while (routing.IsEnd(index) == false)
            {
                uavPath.Path.Add(coordinates[manager.IndexToNode((int)index)]);
                var previousIndex = index;
                index = solution.Value(routing.NextVar(index));
                routeDistance += routing.GetArcCostForVehicle(previousIndex, index, i);
            }

            uavPath.VehicleId = i;
            uavPath.Distance = (double)routeDistance / Scale;
            uavPaths.Add(uavPath);
        }

        return uavPaths;
    }
}