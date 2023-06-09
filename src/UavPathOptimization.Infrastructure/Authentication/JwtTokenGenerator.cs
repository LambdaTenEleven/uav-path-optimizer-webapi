using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UavPathOptimization.Application.Common.Authentication;

namespace UavPathOptimization.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    public string GenerateToken(Guid userId, string firstName, string lastName, string email)
    {
        // TODO: move secrets to configuration
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super-secret-key-123456780000-secret-key")),
            SecurityAlgorithms.HmacSha256
        );

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, firstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, lastName),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        //TODO: move expiration time to configuration
        //TODO: inject DateTimeProvider
        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddDays(7)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}