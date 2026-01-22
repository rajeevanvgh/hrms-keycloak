using Hrms.EmployeeService.Application.Common;
using Hrms.EmployeeService.Application.DTOs;
using MediatR;

namespace Hrms.EmployeeService.Application.UseCases.GetAllEmployees;

public sealed record GetAllEmployeesQuery : IRequest<Result<IEnumerable<EmployeeDto>>>;