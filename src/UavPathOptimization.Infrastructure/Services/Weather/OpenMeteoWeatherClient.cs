using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using UavPathOptimization.Domain.Contracts.Weather;
using UavPathOptimization.Domain.Services;

namespace UavPathOptimization.Infrastructure.Services.Weather;

public class OpenMeteoWeatherClient : IWeatherClient
{
    private readonly HttpClient _client;

    public OpenMeteoWeatherClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<IEnumerable<WindData>> GetWindDataAsync(DateTime startDateTimeUtc, DateTime endDateTimeUtc,
        double latitude, double longitude, CancellationToken cancellationToken = default)
    {
        var startDateFormatted = startDateTimeUtc.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var endDateFormatted = endDateTimeUtc.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        var requestUri =
            $"forecast?latitude={latitude}&longitude={longitude}&hourly=wind_speed_10m,wind_direction_10m&" +
            $"start_date={startDateFormatted}&end_date={endDateFormatted}";

        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

        var response = await _client.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return Enumerable.Empty<WindData>();
        }

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var content = await response.Content.ReadFromJsonAsync<OpenMeteoWeatherResponse>(options, cancellationToken);

        if (content == null)
        {
            return Enumerable.Empty<WindData>();
        }

        var windDataList = new List<WindData>();
        for (var i = 0; i < content.Hourly.Time.Count; i++)
        {
            var dateTime = DateTime.Parse(content.Hourly.Time[i], CultureInfo.InvariantCulture);
            var windSpeed = content.Hourly.WindSpeed10m[i];
            var windDirection = content.Hourly.WindDirection10m[i];
            windDataList.Add(new WindData(dateTime, windSpeed, windDirection));
        }

        return windDataList;
    }
}