using UavPathOptimization.Domain.Entities;

namespace UavPathOptimization.Application.Services;

public interface IUavService
{
    public IEnumerable<Uav> GetAllUavs();

    public Uav GetUavById(Guid id);

    public void AddUav(Uav uav);

    public void EditUav(Uav uav);

    public void DeleteUav(Uav uav);
}