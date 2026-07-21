using FluentAssertions;

using PSP.Payment.Domain.ValueObjects;

using Xunit;

namespace PSP.Payment.ValueObjects;

public class TerminalIdTests
{
    [Theory]
    [InlineData("TERM001")]
    [InlineData("T001")]
    [InlineData("TERMINAL123")]
    public void Create_WithValidTerminalId_ShouldCreateTerminalId(string validTerminalId)
    {
        // Act
        var terminalId = TerminalId.Create(validTerminalId);

        // Assert
        terminalId.Value.Should().Be(validTerminalId);
    }

    [Fact]
    public void Create_WithNullTerminalId_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => TerminalId.Create(null!));
    }

    [Fact]
    public void ImplicitConversion_ToString_ShouldReturnValue()
    {
        // Arrange
        var terminalId = TerminalId.Create("TERM001");

        // Act
        string value = terminalId;

        // Assert
        value.Should().Be("TERM001");
    }

    [Fact]
    public void Equality_ShouldWorkCorrectly()
    {
        // Arrange
        var term1 = TerminalId.Create("TERM001");
        var term2 = TerminalId.Create("TERM001");
        var term3 = TerminalId.Create("TERM002");

        // Assert
        term1.Should().Be(term2);
        term1.Should().NotBe(term3);
    }
}
