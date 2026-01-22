namespace Hrms.EmployeeService.Domain.Events;

public sealed record EmployeeTerminatedEvent(Guid EmployeeId, string Reason) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}