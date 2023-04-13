using GeoCoordinatePortable;
using Google.OrTools.ConstraintSolver;

namespace UavPathOptimization.Application.Services;

public class PathOptimizerService : IPathOptimizerService
{
    private const int SCALE = 100;

    public IList<GeoCoordinate> OptimizePath(IList<GeoCoordinate> path)
    {
        // create distance matrix
        var count = path.Count;
        var matrix = new long[count, count];

        for (var i = 0; i < count; i++)
        {
            for (var j = i; j < count; j++)
            {
                var distance = (long)(path[i].GetDistanceTo(path[j]) * SCALE);
                matrix[i, j] = distance;
                matrix[j, i] = distance;
            }
        }

        // routing model
        var manager = new RoutingIndexManager(matrix.GetLength(0), 1, 0);
        var routing = new RoutingModel(manager);

        // distance callback
        var transitCallbackIndex = routing.RegisterTransitCallback((long fromIndex, long toIndex) =>
           {
               // Convert from routing variable Index to
               // distance matrix NodeIndex.
               var fromNode = manager.IndexToNode(fromIndex);
               var toNode = manager.IndexToNode(toIndex);
               return matrix[fromNode, toNode];
           });

        // set cost of travel
        routing.SetArcCostEvaluatorOfAllVehicles(transitCallbackIndex);

        // set search parameters
        var searchParameters = operations_research_constraint_solver.DefaultRoutingSearchParameters();
        searchParameters.FirstSolutionStrategy = FirstSolutionStrategy.Types.Value.PathCheapestArc;

        // get soulution
        var solution = routing.SolveWithParameters(searchParameters);

        var result = new List<GeoCoordinate>();

        long routeDistance = 0;
        var index = routing.Start(0);
        while (routing.IsEnd(index) == false)
        {
            result.Add(path[manager.IndexToNode((int)index)]);
            var previousIndex = index;
            index = solution.Value(routing.NextVar(index));
            routeDistance += routing.GetArcCostForVehicle(previousIndex, index, 0);
        }

        return result;
    }
}