namespace Hrms.EmployeeService.Domain.Exceptions;

public class EmployeeDomainException : Exception
{
    public EmployeeDomainException(string message) : base(message)
    {
    }

    public EmployeeDomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}