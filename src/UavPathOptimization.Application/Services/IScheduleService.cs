using UavPathOptimization.Domain.Entities;

namespace UavPathOptimization.Application.Services;

public interface IScheduleService
{
    public Schedule GetSchedule();
}