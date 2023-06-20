using ErrorOr;
using GeoCoordinatePortable;
using Google.OrTools.ConstraintSolver;
using MapsterMapper;
using MediatR;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Domain.Contracts;
using UavPathOptimization.Domain.Entities;

namespace UavPathOptimization.Application.UseCases.PathOptimizer.Queries.OptimizePath;

public class OptimizePathHandler :
    IRequestHandler<OptimizePathQuery, ErrorOr<OptimizePathResult>>
{
    private readonly IMapper _mapper;

    public OptimizePathHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    private const int SCALE = 100;

    public Task<ErrorOr<OptimizePathResult>> Handle(OptimizePathQuery request, CancellationToken cancellationToken)
    {
        if (request.Path.Count < 2)
        {
            return Task.FromResult<ErrorOr<OptimizePathResult>>(
                Errors.OptimizePath.InputPathValidationError);
        }

        // map path to GeoCoordinate
        var path = request.Path.Select(x => _mapper.Map<GeoCoordinate>(x)).ToList();

        // create distance matrix
        var count = request.Path.Count;
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

        var finalPath = new List<GeoCoordinate>();

        long routeDistance = 0;
        var index = routing.Start(0);
        while (routing.IsEnd(index) == false)
        {
            finalPath.Add(path[manager.IndexToNode((int)index)]);
            var previousIndex = index;
            index = solution.Value(routing.NextVar(index));
            routeDistance += routing.GetArcCostForVehicle(previousIndex, index, 0);
        }

        var result = new OptimizePathResult(
            finalPath.Select(x => _mapper.Map<GeoCoordinateDto>(x)).ToList(),
            routeDistance / SCALE
        );

        return Task.FromResult<ErrorOr<OptimizePathResult>>(result);
    }
}