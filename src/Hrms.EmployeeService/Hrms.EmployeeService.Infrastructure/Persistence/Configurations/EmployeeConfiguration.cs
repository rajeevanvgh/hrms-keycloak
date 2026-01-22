using Hrms.EmployeeService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hrms.EmployeeService.Infrastructure.Persistence.Configurations;

public sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(e => e.Department)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Position)
            .IsRequired()
            .HasMaxLength(50);

        // Email value object
        builder.OwnsOne(e => e.Email, email =>
        {
            email.Property(em => em.Value)
                .HasColumnName("Email")
                .IsRequired()
                .HasMaxLength(100);

            email.HasIndex(em => em.Value)
                .IsUnique();
        });

        // EmployeeCode value object
        builder.OwnsOne(e => e.EmployeeCode, code =>
        {
            code.Property(c => c.Value)
                .HasColumnName("EmployeeCode")
                .IsRequired()
                .HasMaxLength(20);

            code.HasIndex(c => c.Value)
                .IsUnique();
        });

        // Salary value object
        builder.OwnsOne(e => e.Salary, salary =>
        {
            salary.Property(s => s.Amount)
                .HasColumnName("Salary")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            salary.Property(s => s.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .IsRequired()
                .HasDefaultValue("USD");
        });

        builder.Property(e => e.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.UpdatedAt);

        builder.Ignore(e => e.DomainEvents);
    }
}