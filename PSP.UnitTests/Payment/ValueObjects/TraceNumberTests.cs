using FluentAssertions;

using PSP.Payment.Domain.ValueObjects;

using Xunit;

namespace PSP.Payment.ValueObjects;

public class TraceNumberTests
{
    [Theory]
    [InlineData("123456")]
    [InlineData("000001")]
    [InlineData("999999")]
    public void Create_WithValidTraceNumber_ShouldCreateTraceNumber(string validTraceNumber)
    {
        // Act
        var traceNumber = TraceNumber.Create(validTraceNumber);

        // Assert
        traceNumber.Value.Should().Be(validTraceNumber);
    }

    [Fact]
    public void Create_WithNullTraceNumber_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => TraceNumber.Create(null!));
    }

    [Fact]
    public void ImplicitConversion_ToString_ShouldReturnValue()
    {
        // Arrange
        var traceNumber = TraceNumber.Create("123456");

        // Act
        string value = traceNumber;

        // Assert
        value.Should().Be("123456");
    }

    [Fact]
    public void Equality_ShouldWorkCorrectly()
    {
        // Arrange
        var trace1 = TraceNumber.Create("123456");
        var trace2 = TraceNumber.Create("123456");
        var trace3 = TraceNumber.Create("654321");

        // Assert
        trace1.Should().Be(trace2);
        trace1.Should().NotBe(trace3);
    }
}
