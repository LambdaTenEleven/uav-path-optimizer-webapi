namespace UavPathOptimization.Domain.Common.Settings;

public sealed class PasswordSettings
{
    public const string SectionName = "PasswordSettings";
    public bool RequireDigit { get; init; }
    public bool RequireLowercase { get; init; }
    public bool RequireUppercase { get; init; }
    public bool RequireNonAlphanumeric { get; init; }
    public int RequiredLength { get; init; }
}