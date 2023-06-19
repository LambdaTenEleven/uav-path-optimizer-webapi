namespace UavPathOptimization.Domain.Common;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";
    public string SecretKey { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public int ExpirationTimeInMinutes { get; set; }
}