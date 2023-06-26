using ErrorOr;
using GeoCoordinatePortable;
using Google.OrTools.ConstraintSolver;
using MapsterMapper;
using MediatR;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Domain.Contracts.OptimizePath;
using UavPathOptimization.Domain.Entities.Results;

namespace UavPathOptimization.Application.UseCases.PathOptimization.Queries.OptimizePath;

public class OptimizePathQueryHandler : IRequestHandler<OptimizePathQuery, ErrorOr<OptimizePathResult>>
{
    private readonly IMapper _mapper;

    public OptimizePathQueryHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    private const int SCALE = 100;

    public Task<ErrorOr<OptimizePathResult>> Handle(OptimizePathQuery request, CancellationToken cancellationToken)
    {
        // Map coordinates to GeoCoordinate
        var path = request.Coordinates.Select(x => new GeoCoordinate(x.Latitude, x.Longitude)).ToList();

        // Create distance matrix
        var count = request.Coordinates.Count;
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

        // Create Routing Index Manager
        var manager =
            new RoutingIndexManager(matrix.GetLength(0), request.UAVCount, 0);

        // Create Routing Model.
        var routing = new RoutingModel(manager);

        // Create and register a transit callback.
        int transitCallbackIndex = routing.RegisterTransitCallback((long fromIndex, long toIndex) =>
           {
               // Convert from routing variable Index to
               // distance matrix NodeIndex.
               var fromNode = manager.IndexToNode(fromIndex);
               var toNode = manager.IndexToNode(toIndex);
               return matrix[fromNode, toNode];
           });

        // Define cost of each arc.
        routing.SetArcCostEvaluatorOfAllVehicles(transitCallbackIndex);

        // Add Distance constraint.
        routing.AddDimension(transitCallbackIndex, 0, 30000000, //TODO figure out the exact value for this
                             true, // start cumul to zero
                             "Distance");
        RoutingDimension distanceDimension = routing.GetMutableDimension("Distance");
        distanceDimension.SetGlobalSpanCostCoefficient(100000);

        // Setting first solution heuristic.
        RoutingSearchParameters searchParameters =
            operations_research_constraint_solver.DefaultRoutingSearchParameters();
        searchParameters.FirstSolutionStrategy = FirstSolutionStrategy.Types.Value.PathCheapestArc;

        // Solve the problem.
        var solution = routing.SolveWithParameters(searchParameters);
        if (solution is null)
        {
            return Task.FromResult<ErrorOr<OptimizePathResult>>(Errors.OptimizePath.SolutionError);
        }

        var uavPaths = new List<UAVPath>();

        for (int i = 0; i < request.UAVCount; ++i)
        {
            long routeDistance = 0;
            var index = routing.Start(i);

            var uavPath = new UAVPath();
            while (routing.IsEnd(index) == false)
            {
                uavPath.Path.Add(request.Coordinates[manager.IndexToNode((int)index)]);
                var previousIndex = index;
                index = solution.Value(routing.NextVar(index));
                routeDistance += routing.GetArcCostForVehicle(previousIndex, index, i);
            }

            uavPath.UAVId = i;
            uavPath.Distance = (double)routeDistance / SCALE;
            uavPaths.Add(uavPath);
        }

        var result = new OptimizePathResult(uavPaths);

        return Task.FromResult<ErrorOr<OptimizePathResult>>(result);
    }
}