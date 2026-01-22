using Hrms.EmployeeService.Domain.ValueObjects;
using FluentAssertions;

namespace Hrms.EmployeeService.Domain.Tests.ValueObjects;

public class EmailTests
{
    [Theory]
    [InlineData("user@example.com")]
    [InlineData("john.doe@company.co.uk")]
    [InlineData("test+alias@domain.com")]
    public void From_ShouldCreateEmail_WithValidFormat(string emailAddress)
    {
        // Act
        var email = Email.From(emailAddress);

        // Assert
        email.Should().NotBeNull();
        email.Value.Should().Be(emailAddress.ToLowerInvariant());
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void From_ShouldThrowException_WhenEmailIsNullOrWhitespace(string emailAddress)
    {
        // Act
        var act = () => Email.From(emailAddress);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("email");
    }

    [Theory]
    [InlineData("notanemail")]
    [InlineData("@example.com")]
    [InlineData("user@")]
    [InlineData("user @example.com")]
    public void From_ShouldThrowException_WithInvalidFormat(string emailAddress)
    {
        // Act
        var act = () => Email.From(emailAddress);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid email format*");
    }

    [Fact]
    public void From_ShouldConvertToLowerCase()
    {
        // Arrange
        var mixedCaseEmail = "John.Doe@COMPANY.COM";

        // Act
        var email = Email.From(mixedCaseEmail);

        // Assert
        email.Value.Should().Be("john.doe@company.com");
    }

    [Fact]
    public void Equals_ShouldReturnTrue_ForSameEmailValue()
    {
        // Arrange
        var email1 = Email.From("test@example.com");
        var email2 = Email.From("test@example.com");

        // Act & Assert
        email1.Should().Be(email2);
        (email1 == email2).Should().BeTrue();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_ForDifferentEmailValues()
    {
        // Arrange
        var email1 = Email.From("test1@example.com");
        var email2 = Email.From("test2@example.com");

        // Act & Assert
        email1.Should().NotBe(email2);
        (email1 != email2).Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_ShouldBeSame_ForEqualEmails()
    {
        // Arrange
        var email1 = Email.From("test@example.com");
        var email2 = Email.From("test@example.com");

        // Act & Assert
        email1.GetHashCode().Should().Be(email2.GetHashCode());
    }
}