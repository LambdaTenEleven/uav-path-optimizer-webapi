using UavPathOptimization.Application.Common.Services;

namespace UavPathOptimization.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}