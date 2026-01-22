using Hrms.EmployeeService.Application.Common;
using Hrms.EmployeeService.Application.DTOs;
using Hrms.EmployeeService.Domain.Repositories;
using MediatR;

namespace Hrms.EmployeeService.Application.UseCases.GetAllEmployees;

public sealed class GetAllEmployeesQueryHandler(IEmployeeRepository repository)
    : IRequestHandler<GetAllEmployeesQuery, Result<IEnumerable<EmployeeDto>>>
{
    public async Task<Result<IEnumerable<EmployeeDto>>> Handle(
        GetAllEmployeesQuery request,
        CancellationToken cancellationToken)
    {
        var employees = await repository.GetAllAsync(cancellationToken);
        var dtos = employees.Select(EmployeeDto.FromEntity);

        return Result<IEnumerable<EmployeeDto>>.Success(dtos);
    }
}