namespace Hrms.EmployeeService.Domain.ValueObjects;

public sealed class EmployeeCode(string value) : ValueObject
{
    public string Value { get; } = value;

    public static EmployeeCode Generate()
    {
        var code = $"EMP{DateTime.UtcNow:yyyyMMdd}{Random.Shared.Next(1000, 9999)}";
        return new(code);
    }

    public static EmployeeCode From(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));
        return new(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}