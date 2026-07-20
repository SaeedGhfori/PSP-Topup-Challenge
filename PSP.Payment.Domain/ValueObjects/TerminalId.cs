namespace PSP.Payment.Domain.ValueObjects;

public sealed record TerminalId
{
    public string Value { get; }

    private TerminalId(string value)
    {
        Value = value;
    }

    public static TerminalId Create(string value)
        => new(value);

    public static implicit operator string(TerminalId value)
        => value.Value;
}
