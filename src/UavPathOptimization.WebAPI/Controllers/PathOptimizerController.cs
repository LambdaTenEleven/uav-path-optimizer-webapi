using GeoCoordinatePortable;
using Microsoft.AspNetCore.Mvc;
using UavPathOptimization.Application.Services;
using UavPathOptimization.WebAPI.DTO;

namespace UavPathOptimization.WebAPI.Controllers;

[ApiController]
[Route("api/optimizePath")]
public class PathOptimizerController : ControllerBase
{
    private readonly IPathOptimizerService _pathOptimizerService;

    public PathOptimizerController(IPathOptimizerService pathOptimizerService)
    {
        _pathOptimizerService = pathOptimizerService;
    }

    [HttpPost]
    public IActionResult Post([FromBody] IList<GeoCoordinateDto> path)
    {
        if (path == null || path.Count < 2)
        {
            return BadRequest("Invalid input. Path must contain at least 2 GeoCoordinate objects.");
        }

        var mapper = new GeoCoordinateMapper();
        var newPath = path
            .Select(x => mapper.GeoCoordinateDtoToGeoCoordinate(x))
            .ToList();

        var result = _pathOptimizerService
            .OptimizePath(newPath)
            .Select(x => mapper.GeoCoordinateToGeoCoordinateDto(x));

        return Ok(result);
    }
}