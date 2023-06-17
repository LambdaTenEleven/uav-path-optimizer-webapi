using UavPathOptimization.Application.Common.Persistence;
using UavPathOptimization.Domain.Entities;

namespace UavPathOptimization.Infrastructure.Persistence;

public class InMemoryUserRepository : IUserRepository
{
    private static readonly List<User> _users = new();

    public void Add(User user)
    {
        _users.Add(user);
    }

    public User? GetUserByEmail(string email)
    {
        return _users.FirstOrDefault(u => u.Email == email);
    }
}