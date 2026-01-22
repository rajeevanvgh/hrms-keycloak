namespace Hrms.EmployeeService.Domain.ValueObjects;

public sealed class Salary(decimal amount, string currency = "USD") : ValueObject
{
    public decimal Amount { get; } = Validate(amount);
    public string Currency { get; } = currency;

    public static Salary From(decimal amount, string currency = "USD")
        => new(amount, currency);

    private static decimal Validate(decimal amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount, nameof(amount));
        return amount;
    }

    public Salary Increase(decimal percentage)
        => new(Amount * (1 + percentage / 100), Currency);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}