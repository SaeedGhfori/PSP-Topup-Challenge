namespace PSP.Payment.Domain.ValueObjects;

public sealed record Money
{
    public decimal Value { get; }

    private Money(decimal value)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        Value = value;
    }

    public static Money Create(decimal value)
        => new(value);

    public static implicit operator decimal(Money money)
        => money.Value;
}
