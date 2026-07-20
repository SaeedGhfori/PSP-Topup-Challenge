public sealed record Money
{
    public decimal Value { get; }

    private Money(decimal value)
    {
        // Validation
        Value = value;
    }

    public static Money Create(decimal value)
        => new(value);

    public static implicit operator decimal(Money money)
        => money.Value;
}
