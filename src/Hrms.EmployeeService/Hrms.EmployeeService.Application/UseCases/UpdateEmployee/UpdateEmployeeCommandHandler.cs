using Hrms.EmployeeService.Application.Common;
using Hrms.EmployeeService.Domain.Repositories;
using MediatR;

namespace Hrms.EmployeeService.Application.UseCases.UpdateEmployee;

public sealed class UpdateEmployeeCommandHandler(
    IEmployeeRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateEmployeeCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        UpdateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await repository.GetByIdAsync(request.EmployeeId, cancellationToken);

        if (employee is null)
            return Result<bool>.Failure("Employee not found");

        employee.UpdatePersonalInfo(
            request.FirstName,
            request.LastName,
            request.PhoneNumber);

        await repository.UpdateAsync(employee, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}