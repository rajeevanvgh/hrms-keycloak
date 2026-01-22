using Hrms.EmployeeService.Application.Common;
using Hrms.EmployeeService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hrms.EmployeeService.Infrastructure.Persistence;

public sealed class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Employee> Employees => Set<Employee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entities = ChangeTracker
            .Entries<Employee>()
            .Where(x => x.Entity.DomainEvents.Count > 0)
            .ToList();

        var events = entities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        entities.ForEach(e => e.Entity.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        // TODO: Publish domain events here if using event bus

        return result;
    }
}