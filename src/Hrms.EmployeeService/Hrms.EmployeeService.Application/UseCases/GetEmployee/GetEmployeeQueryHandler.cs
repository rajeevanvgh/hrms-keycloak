using Hrms.EmployeeService.Application.Common;
using Hrms.EmployeeService.Application.DTOs;
using Hrms.EmployeeService.Domain.Repositories;
using MediatR;

namespace Hrms.EmployeeService.Application.UseCases.GetEmployee;

public sealed class GetEmployeeQueryHandler(IEmployeeRepository repository)
    : IRequestHandler<GetEmployeeQuery, Result<EmployeeDto>>
{
    public async Task<Result<EmployeeDto>> Handle(
        GetEmployeeQuery request,
        CancellationToken cancellationToken)
    {
        var employee = await repository.GetByIdAsync(request.EmployeeId, cancellationToken);

        if (employee is null)
            return Result<EmployeeDto>.Failure("Employee not found");

        return Result<EmployeeDto>.Success(EmployeeDto.FromEntity(employee));
    }
}