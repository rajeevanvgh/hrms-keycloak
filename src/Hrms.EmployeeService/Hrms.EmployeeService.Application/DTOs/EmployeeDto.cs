using Hrms.EmployeeService.Domain.Entities;

namespace Hrms.EmployeeService.Application.DTOs;

public sealed record EmployeeDto
{
    public required Guid Id { get; init; }
    public required string EmployeeCode { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public string? PhoneNumber { get; init; }
    public required string Department { get; init; }
    public required string Position { get; init; }
    public required DateTime HireDate { get; init; }
    public required decimal Salary { get; init; }
    public required string Status { get; init; }

    public static EmployeeDto FromEntity(Employee e) => new()
    {
        Id = e.Id,
        EmployeeCode = e.EmployeeCode.Value,
        FirstName = e.FirstName,
        LastName = e.LastName,
        Email = e.Email.Value,
        PhoneNumber = e.PhoneNumber,
        Department = e.Department,
        Position = e.Position,
        HireDate = e.HireDate,
        Salary = e.Salary.Amount,
        Status = e.Status.ToString()
    };
}