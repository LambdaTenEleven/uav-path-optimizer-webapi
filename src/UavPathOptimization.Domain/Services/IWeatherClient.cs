using UavPathOptimization.Domain.Contracts.Weather;

namespace UavPathOptimization.Domain.Services;

public interface IWeatherClient
{
    public Task<IEnumerable<WindData>> GetWindDataAsync(DateTime startDateTimeUtc, DateTime endDateTimeUtc, double latitude,
        double longitude, CancellationToken cancellationToken = default);
}