using System;

using FluentAssertions;

using Xunit;

namespace PSP.Topup.ValueObjects;

public class PhoneNumberTests
{
    [Theory]
    [InlineData("09123456789")]
    [InlineData("09351234567")]
    [InlineData("02112345678")]
    public void Create_WithValidPhoneNumber_ShouldCreatePhoneNumber(string validPhoneNumber)
    {
        // Act
        var phoneNumber = PhoneNumber.Create(validPhoneNumber);

        // Assert
        phoneNumber.Value.Should().Be(validPhoneNumber);
    }

    [Theory]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("091234567890")]
    [InlineData("0211234567")]
    [InlineData("abc12345678")]
    public void Create_WithInvalidPhoneNumber_ShouldThrowArgumentException(string invalidPhoneNumber)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => PhoneNumber.Create(invalidPhoneNumber));
    }

    [Fact]
    public void ImplicitConversion_ToString_ShouldReturnValue()
    {
        // Arrange
        var phoneNumber = PhoneNumber.Create("09123456789");

        // Act
        string value = phoneNumber;

        // Assert
        value.Should().Be("09123456789");
    }

    [Fact]
    public void Equality_ShouldWorkCorrectly()
    {
        // Arrange
        var phone1 = PhoneNumber.Create("09123456789");
        var phone2 = PhoneNumber.Create("09123456789");
        var phone3 = PhoneNumber.Create("09351234567");

        // Assert
        phone1.Should().Be(phone2);
        phone1.Should().NotBe(phone3);
    }
}
