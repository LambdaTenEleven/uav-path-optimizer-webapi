using UavPathOptimization.Domain.Entities;

namespace UavPathOptimization.Application.Common.Persistence;

public interface IUserRepository
{
    void Add(User user);
    User? GetUserByEmail(string email);
}