using Hrms.EmployeeService.Domain.Entities;
using Hrms.EmployeeService.Domain.Repositories;
using Hrms.EmployeeService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Hrms.EmployeeService.Infrastructure.Persistence.Repositories;

public sealed class EmployeeRepository(ApplicationDbContext context)
    : IEmployeeRepository
{
    public async Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Employees.FindAsync([id], cancellationToken);

    public async Task<Employee?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
        => await context.Employees
            .FirstOrDefaultAsync(e => e.Email.Value == email.Value, cancellationToken);

    public async Task<IEnumerable<Employee>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.Employees
            .Where(e => e.Status == EmployeeStatus.Active)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Employee>> GetByDepartmentAsync(
        string department,
        CancellationToken cancellationToken = default)
        => await context.Employees
            .Where(e => e.Department == department && e.Status == EmployeeStatus.Active)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Employee employee, CancellationToken cancellationToken = default)
        => await context.Employees.AddAsync(employee, cancellationToken);

    public Task UpdateAsync(Employee employee, CancellationToken cancellationToken = default)
    {
        context.Employees.Update(employee);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Employee employee, CancellationToken cancellationToken = default)
    {
        context.Employees.Remove(employee);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Employees.AnyAsync(e => e.Id == id, cancellationToken);
}