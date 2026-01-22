namespace Hrms.EmployeeService.Domain.Events;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}