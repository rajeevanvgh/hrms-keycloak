using Hrms.EmployeeService.Application.Common;
using Hrms.EmployeeService.Application.DTOs;
using MediatR;

namespace Hrms.EmployeeService.Application.UseCases.CreateEmployee;

public sealed record CreateEmployeeCommand(
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    string Department,
    string Position,
    DateTime HireDate,
    decimal Salary
) : IRequest<Result<EmployeeDto>>;