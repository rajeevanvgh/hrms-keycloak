namespace Hrms.EmployeeService.Api.Models;

public sealed record CreateEmployeeRequest
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public string? PhoneNumber { get; init; }
    public required string Department { get; init; }
    public required string Position { get; init; }
    public required DateTime HireDate { get; init; }
    public required decimal Salary { get; init; }
}