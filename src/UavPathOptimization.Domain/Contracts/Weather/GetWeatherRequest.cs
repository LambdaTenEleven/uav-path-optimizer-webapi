namespace UavPathOptimization.Domain.Contracts.Weather;

public record GetWeatherRequest(DateTime StartDateTimeUtc, DateTime EndDateTimeUtc, double Latitude, double Longitude);