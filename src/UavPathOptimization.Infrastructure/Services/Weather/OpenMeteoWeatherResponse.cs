using System.Text.Json.Serialization;

namespace UavPathOptimization.Infrastructure.Services.Weather;

public record OpenMeteoWeatherResponse(
    double Latitude,
    double Longitude,
    double GenerationTimeMs,
    int UtcOffsetSeconds,
    string Timezone,
    string TimezoneAbbreviation,
    double Elevation,
    HourlyUnits HourlyUnits,
    HourlyForecast Hourly);

public record HourlyUnits(
    string Time,
    string WindSpeed10m,
    string WindDirection10m);

public record HourlyForecast
{
    public IList<string> Time { get; set; }

    [JsonPropertyName("wind_speed_10m")]
    public IList<double> WindSpeed10m { get; set; }

    [JsonPropertyName("wind_direction_10m")]
    public IList<int> WindDirection10m { get; set; }
}