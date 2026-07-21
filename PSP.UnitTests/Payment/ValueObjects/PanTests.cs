using System;

using FluentAssertions;

using PSP.Payment.Domain.ValueObjects;

using Xunit;

namespace PSP.Payment.ValueObjects;

public class PanTests
{
    [Theory]
    [InlineData("1234567890123456")]
    [InlineData("4111111111111111")]
    [InlineData("5500000000000004")]
    public void Create_WithValidPan_ShouldCreatePan(string validPan)
    {
        // Act
        var pan = Pan.Create(validPan);

        // Assert
        pan.Value.Should().Be(validPan);
    }

    [Theory]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("123456789012345")]
    [InlineData("12345678901234567")]
    [InlineData("abcd123456789012")]
    public void Create_WithInvalidPan_ShouldThrowArgumentException(string invalidPan)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Pan.Create(invalidPan));
    }

    [Fact]
    public void ImplicitConversion_ToString_ShouldReturnValue()
    {
        // Arrange
        var pan = Pan.Create("1234567890123456");

        // Act
        string value = pan;

        // Assert
        value.Should().Be("1234567890123456");
    }

    [Fact]
    public void Equality_ShouldWorkCorrectly()
    {
        // Arrange
        var pan1 = Pan.Create("1234567890123456");
        var pan2 = Pan.Create("1234567890123456");
        var pan3 = Pan.Create("4111111111111111");

        // Assert
        pan1.Should().Be(pan2);
        pan1.Should().NotBe(pan3);
    }
}
