using MediatR;
using Microsoft.AspNetCore.Mvc;
using UavPathOptimization.Application.UseCases.PathOptimizer.Queries;
using UavPathOptimization.WebAPI.DTO;
using UavPathOptimization.WebAPI.Extensions;

namespace UavPathOptimization.WebAPI.Controllers;

[ApiController]
[Route("api/optimizePath")]
public class PathOptimizerController : ApiController
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

        return result.Match(
            result => Ok(result.Select(x => mapper.GeoCoordinateToGeoCoordinateDto(x))),
                errors => Problem(errors)
        );
    }
}