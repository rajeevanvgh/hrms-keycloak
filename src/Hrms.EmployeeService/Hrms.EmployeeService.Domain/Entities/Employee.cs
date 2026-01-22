using Hrms.EmployeeService.Domain.ValueObjects;
using Hrms.EmployeeService.Domain.Events;
using Hrms.EmployeeService.Domain.Exceptions;

namespace Hrms.EmployeeService.Domain.Entities;

public class Employee
{
    public required Guid Id { get; init; }
    public required EmployeeCode EmployeeCode { get; init; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required Email Email { get; set; }
    public string? PhoneNumber { get; set; }
    public required string Department { get; set; }
    public required string Position { get; set; }
    public required DateTime HireDate { get; init; }
    public required Salary Salary { get; set; }
    public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

    private Employee() { }

    public static Employee Create(
        string firstName,
        string lastName,
        Email email,
        string? phoneNumber,
        string department,
        string position,
        DateTime hireDate,
        Salary salary)
    {
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            EmployeeCode = EmployeeCode.Generate(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PhoneNumber = phoneNumber,
            Department = department,
            Position = position,
            HireDate = hireDate,
            Salary = salary,
            Status = EmployeeStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        employee._domainEvents.Add(
            new EmployeeCreatedEvent(employee.Id, employee.Email.Value)
        );

        return employee;
    }

    public void UpdatePersonalInfo(string firstName, string lastName, string? phoneNumber)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName, nameof(firstName));
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName, nameof(lastName));

        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Promote(string newPosition, Salary newSalary)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newPosition, nameof(newPosition));

        if (Status is not EmployeeStatus.Active)
            throw new EmployeeDomainException("Cannot promote inactive employee");

        Position = newPosition;
        Salary = newSalary;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Terminate(string reason)
    {
        if (Status == EmployeeStatus.Terminated)
            throw new EmployeeDomainException("Employee already terminated");

        Status = EmployeeStatus.Terminated;
        UpdatedAt = DateTime.UtcNow;
        _domainEvents.Add(new EmployeeTerminatedEvent(Id, reason));
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}

public enum EmployeeStatus
{
    Active,
    OnLeave,
    Suspended,
    Terminated
}