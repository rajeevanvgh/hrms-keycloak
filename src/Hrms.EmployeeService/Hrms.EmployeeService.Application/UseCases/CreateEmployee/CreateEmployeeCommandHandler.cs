using Hrms.EmployeeService.Application.Common;
using Hrms.EmployeeService.Application.DTOs;
using Hrms.EmployeeService.Domain.Entities;
using Hrms.EmployeeService.Domain.Repositories;
using Hrms.EmployeeService.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hrms.EmployeeService.Application.UseCases.CreateEmployee;

public sealed class CreateEmployeeCommandHandler(
    IEmployeeRepository repository,
    IUnitOfWork unitOfWork,
    ILogger<CreateEmployeeCommandHandler> logger)
    : IRequestHandler<CreateEmployeeCommand, Result<EmployeeDto>>
{
    public async Task<Result<EmployeeDto>> Handle(
        CreateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var email = Email.From(request.Email);

            var existing = await repository.GetByEmailAsync(email, cancellationToken);
            if (existing is not null)
                return Result<EmployeeDto>.Failure("Employee with this email already exists");

            var salary = Salary.From(request.Salary);

            var employee = Employee.Create(
                request.FirstName,
                request.LastName,
                email,
                request.PhoneNumber,
                request.Department,
                request.Position,
                request.HireDate,
                salary
            );

            await repository.AddAsync(employee, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Created employee {EmployeeId} - {Name}",
                employee.Id,
                $"{employee.FirstName} {employee.LastName}");

            return Result<EmployeeDto>.Success(EmployeeDto.FromEntity(employee));
        }
        catch (ArgumentException ex)
        {
            return Result<EmployeeDto>.Failure(ex.Message);
        }
    }
}