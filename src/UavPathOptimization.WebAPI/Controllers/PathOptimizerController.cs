using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UavPathOptimization.Application.UseCases.PathOptimizer.Queries;
using UavPathOptimization.Domain.Contracts;
using UavPathOptimization.Domain.Contracts.OptimizePath;

namespace UavPathOptimization.WebAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/optimizePath")]
public class PathOptimizerController : ApiController
{
    private readonly IMediator _mediator;

    public PathOptimizerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OptimizePathResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] IList<GeoCoordinateDto> path)
    {
        var command = new OptimizePathQuery(path);
        var result = await _mediator.Send(command);

        return result.Match(
            result => Ok(result),
                errors => Problem(errors)
        );
    }
}