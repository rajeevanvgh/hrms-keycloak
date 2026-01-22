using Hrms.EmployeeService.Domain.Entities;
using Hrms.EmployeeService.Domain.Events;
using Hrms.EmployeeService.Domain.Exceptions;
using Hrms.EmployeeService.Domain.ValueObjects;
using FluentAssertions;

namespace Hrms.EmployeeService.Domain.Tests.Entities;

public class EmployeeTests
{
    [Fact]
    public void Create_ShouldCreateEmployeeWithValidData()
    {
        // Arrange
        var email = Email.From("john.doe@company.com");
        var salary = Salary.From(75000);

        // Act
        var employee = Employee.Create(
            "John",
            "Doe",
            email,
            "+1234567890",
            "IT",
            "Software Engineer",
            DateTime.UtcNow.AddMonths(-1),
            salary
        );

        // Assert
        employee.Should().NotBeNull();
        employee.Id.Should().NotBeEmpty();
        employee.FirstName.Should().Be("John");
        employee.LastName.Should().Be("Doe");
        employee.Email.Should().Be(email);
        employee.Status.Should().Be(EmployeeStatus.Active);
        employee.EmployeeCode.Should().NotBeNull();
        employee.DomainEvents.Should().HaveCount(1);
        employee.DomainEvents.First().Should().BeOfType<EmployeeCreatedEvent>();
    }

    [Fact]
    public void UpdatePersonalInfo_ShouldUpdateEmployeeDetails()
    {
        // Arrange
        var employee = CreateTestEmployee();
        var originalUpdatedAt = employee.UpdatedAt;

        // Act
        employee.UpdatePersonalInfo("Jane", "Smith", "+9876543210");

        // Assert
        employee.FirstName.Should().Be("Jane");
        employee.LastName.Should().Be("Smith");
        employee.PhoneNumber.Should().Be("+9876543210");
        employee.UpdatedAt.Should().BeAfter(originalUpdatedAt ?? DateTime.MinValue);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdatePersonalInfo_ShouldThrowException_WhenFirstNameIsInvalid(string firstName)
    {
        // Arrange
        var employee = CreateTestEmployee();

        // Act
        var act = () => employee.UpdatePersonalInfo(firstName, "Doe", "+1234567890");

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Promote_ShouldUpdatePositionAndSalary()
    {
        // Arrange
        var employee = CreateTestEmployee();
        var newSalary = Salary.From(95000);

        // Act
        employee.Promote("Senior Engineer", newSalary);

        // Assert
        employee.Position.Should().Be("Senior Engineer");
        employee.Salary.Should().Be(newSalary);
        employee.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Promote_ShouldThrowException_WhenEmployeeIsNotActive()
    {
        // Arrange
        var employee = CreateTestEmployee();
        employee.Terminate("Test termination");
        var newSalary = Salary.From(95000);

        // Act
        var act = () => employee.Promote("Senior Engineer", newSalary);

        // Assert
        act.Should().Throw<EmployeeDomainException>()
            .WithMessage("Cannot promote inactive employee");
    }

    [Fact]
    public void Terminate_ShouldChangeStatusAndRaiseDomainEvent()
    {
        // Arrange
        var employee = CreateTestEmployee();
        var initialEventCount = employee.DomainEvents.Count;

        // Act
        employee.Terminate("Contract ended");

        // Assert
        employee.Status.Should().Be(EmployeeStatus.Terminated);
        employee.UpdatedAt.Should().NotBeNull();
        employee.DomainEvents.Should().HaveCount(initialEventCount + 1);
        employee.DomainEvents.Last().Should().BeOfType<EmployeeTerminatedEvent>();
    }

    [Fact]
    public void Terminate_ShouldThrowException_WhenAlreadyTerminated()
    {
        // Arrange
        var employee = CreateTestEmployee();
        employee.Terminate("First termination");

        // Act
        var act = () => employee.Terminate("Second termination");

        // Assert
        act.Should().Throw<EmployeeDomainException>()
            .WithMessage("Employee already terminated");
    }

    [Fact]
    public void ClearDomainEvents_ShouldRemoveAllEvents()
    {
        // Arrange
        var employee = CreateTestEmployee();
        employee.DomainEvents.Should().NotBeEmpty();

        // Act
        employee.ClearDomainEvents();

        // Assert
        employee.DomainEvents.Should().BeEmpty();
    }

    private static Employee CreateTestEmployee()
    {
        return Employee.Create(
            "John",
            "Doe",
            Email.From("john.doe@company.com"),
            "+1234567890",
            "IT",
            "Software Engineer",
            DateTime.UtcNow.AddMonths(-1),
            Salary.From(75000)
        );
    }
}