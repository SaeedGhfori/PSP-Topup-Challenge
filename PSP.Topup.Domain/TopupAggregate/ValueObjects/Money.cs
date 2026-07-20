using PSP.Topup.Domain.Common;

namespace PSP.Topup.Domain.TopupAggregate.ValueObjects;

/// <summary>
/// Represents a monetary value.
/// </summary>
public sealed class Money : ValueObject
{
    public decimal Value { get; }

    public Money(decimal value)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
        => Value.ToString("0");
}
