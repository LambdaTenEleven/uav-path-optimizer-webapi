using MediatR;
using Microsoft.AspNetCore.Mvc;
using UavPathOptimization.Application.UseCases.PathOptimizer.Queries;
using UavPathOptimization.WebAPI.DTO;

namespace UavPathOptimization.WebAPI.Controllers;

[ApiController]
[Route("api/optimizePath")]
public class PathOptimizerController : ControllerBase
{
    private readonly IMediator _mediator;

    public PathOptimizerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<GeoCoordinateDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] IList<GeoCoordinateDto> path)
    {
        var mapper = new GeoCoordinateMapper();
        var newPath = path
            .Select(x => mapper.GeoCoordinateDtoToGeoCoordinate(x))
            .ToList();

        var command = new OptimizePathQuery(newPath);
        var result = await _mediator.Send(command);

        if (result.IsFailed)
        {
            return BadRequest(result.Errors.First().Message);
        }

        return Ok(result.Value.Select(x => mapper.GeoCoordinateToGeoCoordinateDto(x)));
    }
}