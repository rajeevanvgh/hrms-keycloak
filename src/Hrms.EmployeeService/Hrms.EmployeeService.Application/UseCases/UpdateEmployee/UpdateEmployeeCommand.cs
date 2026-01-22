using Hrms.EmployeeService.Application.Common;
using MediatR;

namespace Hrms.EmployeeService.Application.UseCases.UpdateEmployee;

public sealed record UpdateEmployeeCommand(
    Guid EmployeeId,
    string FirstName,
    string LastName,
    string? PhoneNumber
) : IRequest<Result<bool>>;