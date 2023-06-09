namespace UavPathOptimization.Application.Common.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string firstName, string lastName, string email);
}