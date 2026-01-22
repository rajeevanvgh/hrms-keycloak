using Hrms.EmployeeService.Domain.ValueObjects;
using FluentAssertions;

namespace Hrms.EmployeeService.Domain.Tests.ValueObjects;

public class EmployeeCodeTests
{
    [Fact]
    public void Generate_ShouldCreateUniqueCode()
    {
        // Act
        var code1 = EmployeeCode.Generate();
        var code2 = EmployeeCode.Generate();

        // Assert
        code1.Should().NotBeNull();
        code2.Should().NotBeNull();
        code1.Value.Should().NotBe(code2.Value);
    }

    [Fact]
    public void Generate_ShouldStartWithEMP()
    {
        // Act
        var code = EmployeeCode.Generate();

        // Assert
        code.Value.Should().StartWith("EMP");
    }

    [Fact]
    public void Generate_ShouldContainCurrentDate()
    {
        // Arrange
        var today = DateTime.UtcNow.ToString("yyyyMMdd");

        // Act
        var code = EmployeeCode.Generate();

        // Assert
        code.Value.Should().Contain(today);
    }

    [Fact]
    public void From_ShouldCreateEmployeeCode_WithValidValue()
    {
        // Arrange
        var value = "EMP20240115001";

        // Act
        var code = EmployeeCode.From(value);

        // Assert
        code.Value.Should().Be(value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void From_ShouldThrowException_WhenValueIsNullOrWhitespace(string value)
    {
        // Act
        var act = () => EmployeeCode.From(value);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Equals_ShouldReturnTrue_ForSameCodeValue()
    {
        // Arrange
        var code1 = EmployeeCode.From("EMP20240115001");
        var code2 = EmployeeCode.From("EMP20240115001");

        // Act & Assert
        code1.Should().Be(code2);
    }
}