using UavPathOptimization.Domain.Entities;

namespace UavPathOptimization.Application.Common.Persistance;

public interface IUserRepository
{
    void Add(User user);
    User? GetUserByEmail(string email);
}