using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UavPathOptimization.Application.UseCases.PathOptimization.Queries.OptimizePath;
using UavPathOptimization.Domain.Contracts.OptimizePath;
using UavPathOptimization.WebAPI.Common;

namespace UavPathOptimization.WebAPI.Controllers;

[ApiController]
[Route("api/optimize_path")]
public class PathOptimizerController : ApiController
{
    private readonly IMediator _mediator;

    private readonly IMapper _mapper;

    public PathOptimizerController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OptimizePathResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] OptimizePathRequest request)
    {
        //var query = _mapper.Map<OptimizePathQuery>(request);
        var query = new OptimizePathQuery(request.UAVCount, request.Coordinates);
        var result = await _mediator.Send(query);

        return result.Match(
            ok => Ok(new OptimizePathResponse(ok.UavPaths)),
                errors => Problem(errors)
        );
    }
}