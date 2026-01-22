using Hrms.EmployeeService.Application.Common;
using Hrms.EmployeeService.Domain.Repositories;
using MediatR;

namespace Hrms.EmployeeService.Application.UseCases.TerminateEmployee;

public sealed class TerminateEmployeeCommandHandler(
    IEmployeeRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<TerminateEmployeeCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        TerminateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await repository.GetByIdAsync(request.EmployeeId, cancellationToken);

        if (employee is null)
            return Result<bool>.Failure("Employee not found");

        employee.Terminate(request.Reason);

        await repository.UpdateAsync(employee, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}