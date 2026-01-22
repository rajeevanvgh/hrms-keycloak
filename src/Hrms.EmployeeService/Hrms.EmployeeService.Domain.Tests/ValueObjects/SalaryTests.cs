using Hrms.EmployeeService.Domain.ValueObjects;
using FluentAssertions;

namespace Hrms.EmployeeService.Domain.Tests.ValueObjects;

public class SalaryTests
{
    [Theory]
    [InlineData(50000)]
    [InlineData(100000.50)]
    [InlineData(0)]
    public void From_ShouldCreateSalary_WithValidAmount(decimal amount)
    {
        // Act
        var salary = Salary.From(amount);

        // Assert
        salary.Should().NotBeNull();
        salary.Amount.Should().Be(amount);
        salary.Currency.Should().Be("USD");
    }

    [Fact]
    public void From_ShouldCreateSalary_WithCustomCurrency()
    {
        // Act
        var salary = Salary.From(50000, "EUR");

        // Assert
        salary.Amount.Should().Be(50000);
        salary.Currency.Should().Be("EUR");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-1000)]
    public void From_ShouldThrowException_WhenAmountIsNegative(decimal amount)
    {
        // Act
        var act = () => Salary.From(amount);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("amount");
    }

    [Theory]
    [InlineData(100000, 10, 110000)]
    [InlineData(50000, 20, 60000)]
    [InlineData(75000, 5, 78750)]
    public void Increase_ShouldCalculateCorrectly(decimal original, decimal percentage, decimal expected)
    {
        // Arrange
        var salary = Salary.From(original);

        // Act
        var increased = salary.Increase(percentage);

        // Assert
        increased.Amount.Should().Be(expected);
        increased.Currency.Should().Be(salary.Currency);
    }

    [Fact]
    public void Equals_ShouldReturnTrue_ForSameSalaryValue()
    {
        // Arrange
        var salary1 = Salary.From(75000);
        var salary2 = Salary.From(75000);

        // Act & Assert
        salary1.Should().Be(salary2);
    }

    [Fact]
    public void Equals_ShouldReturnFalse_ForDifferentCurrency()
    {
        // Arrange
        var salary1 = Salary.From(75000, "USD");
        var salary2 = Salary.From(75000, "EUR");

        // Act & Assert
        salary1.Should().NotBe(salary2);
    }
}