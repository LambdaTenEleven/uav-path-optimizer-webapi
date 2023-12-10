using Microsoft.AspNetCore.Mvc;
using UavPathOptimization.Domain.Contracts.Weather;
using UavPathOptimization.Domain.Services;
using UavPathOptimization.WebAPI.Common;

namespace UavPathOptimization.WebAPI.Controllers;

[Route("api/weather")]
public class WeatherController : ApiController
{
    private readonly IWeatherClient _weatherService;

    public WeatherController(IWeatherClient weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WindData>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetWeather([FromQuery] GetWeatherRequest request)
    {
        var weatherData = await _weatherService.GetWindDataAsync(request.StartDateTimeUtc, request.EndDateTimeUtc,
            request.Latitude, request.Longitude);

        return Ok(weatherData);
    }
}