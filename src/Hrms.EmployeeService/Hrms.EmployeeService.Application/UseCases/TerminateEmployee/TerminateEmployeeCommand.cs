using Hrms.EmployeeService.Application.Common;
using MediatR;

namespace Hrms.EmployeeService.Application.UseCases.TerminateEmployee;

public sealed record TerminateEmployeeCommand(
    Guid EmployeeId,
    string Reason
) : IRequest<Result<bool>>;