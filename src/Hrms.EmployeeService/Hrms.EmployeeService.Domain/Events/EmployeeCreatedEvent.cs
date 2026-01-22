namespace Hrms.EmployeeService.Domain.Events;

public sealed record EmployeeCreatedEvent(Guid EmployeeId, string Email) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}