using FluentResults;
using GeoCoordinatePortable;
using Google.OrTools.ConstraintSolver;
using MediatR;

namespace UavPathOptimization.Application.UseCases.PathOptimizer.Queries;

public class OptimizePathHandler :
    IRequestHandler<OptimizePathQuery, Result<IList<GeoCoordinate>>>
{
    private const int SCALE = 100;

    public Task<Result<IList<GeoCoordinate>>> Handle(OptimizePathQuery request, CancellationToken cancellationToken)
    {
        if (request.path.Count < 2)
        {
            var error = Result.Fail(new Error("Path must include more than 2 GeoCoordinate."));
            return Task.FromResult<Result<IList<GeoCoordinate>>>(error);
        }

        // create distance matrix
        var count = request.path.Count;
        var matrix = new long[count, count];

        for (var i = 0; i < count; i++)
        {
            for (var j = i; j < count; j++)
            {
                var distance = (long)(request.path[i].GetDistanceTo(request.path[j]) * SCALE);
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

        var finalPath = new List<GeoCoordinate>();

        long routeDistance = 0; //TODO use it somewhere
        var index = routing.Start(0);
        while (routing.IsEnd(index) == false)
        {
            finalPath.Add(request.path[manager.IndexToNode((int)index)]);
            var previousIndex = index;
            index = solution.Value(routing.NextVar(index));
            routeDistance += routing.GetArcCostForVehicle(previousIndex, index, 0);
        }

        var result = Result.Ok((IList<GeoCoordinate>)finalPath);

        return Task.FromResult(result);
    }
}