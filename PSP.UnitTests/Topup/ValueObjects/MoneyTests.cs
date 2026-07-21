using System;

using FluentAssertions;

using PSP.Payment.Domain.ValueObjects;

using Xunit;

namespace PSP.Topup.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void Create_WithValidAmount_ShouldCreateMoney()
    {
        // Arrange
        decimal validAmount = 100000;

        // Act
        var money = Money.Create(validAmount);

        // Assert
        money.Value.Should().Be(validAmount);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1000)]
    [InlineData(-1)]
    public void Create_WithInvalidAmount_ShouldThrowArgumentOutOfRangeException(decimal invalidAmount)
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => Money.Create(invalidAmount));
    }

    [Fact]
    public void ImplicitConversion_ToDecimal_ShouldReturnValue()
    {
        // Arrange
        var money = Money.Create(50000);

        // Act
        decimal value = money;

        // Assert
        value.Should().Be(50000);
    }

    [Fact]
    public void Equality_ShouldWorkCorrectly()
    {
        // Arrange
        var money1 = Money.Create(100000);
        var money2 = Money.Create(100000);
        var money3 = Money.Create(200000);

        // Assert
        money1.Should().Be(money2);
        money1.Should().NotBe(money3);
    }
}
