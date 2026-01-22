using Hrms.EmployeeService.Application.Common;
using Hrms.EmployeeService.Application.DTOs;
using MediatR;

namespace Hrms.EmployeeService.Application.UseCases.GetEmployee;

public sealed record GetEmployeeQuery(Guid EmployeeId) : IRequest<Result<EmployeeDto>>;