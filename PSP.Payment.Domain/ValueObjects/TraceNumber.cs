namespace PSP.Payment.Domain.ValueObjects;

public sealed record TraceNumber
{
    public string Value { get; }

    private TraceNumber(string value)
    {
        Value = value;
    }

    public static TraceNumber Create(string value)
        => new(value);

    public static implicit operator string(TraceNumber value)
        => value.Value;
}
