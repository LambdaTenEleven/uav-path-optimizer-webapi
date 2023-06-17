using UavPathOptimization.Domain.Entities;

namespace UavPathOptimization.Application.Common.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}