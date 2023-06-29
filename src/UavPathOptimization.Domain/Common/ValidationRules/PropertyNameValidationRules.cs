namespace UavPathOptimization.Domain.Common.ValidationRules;

public static class PropertyNameValidationRules
{
    public static bool MustBeValidPropertyName<T>(string? property)
    {
        if (string.IsNullOrEmpty(property))
            return true;

        var propertyNames = typeof(T).GetProperties().Select(p => p.Name);

        return propertyNames.Contains(property);
    }
}