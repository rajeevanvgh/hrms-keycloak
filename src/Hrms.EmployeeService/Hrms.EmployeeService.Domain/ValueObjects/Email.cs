namespace Hrms.EmployeeService.Domain.ValueObjects;

public sealed class Email(string value) : ValueObject
{
    public string Value { get; } = Validate(value);

    public static Email From(string email) => new(email);

    private static string Validate(string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));

        if (!IsValidEmail(email))
            throw new ArgumentException("Invalid email format", nameof(email));

        return email.ToLowerInvariant();
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}