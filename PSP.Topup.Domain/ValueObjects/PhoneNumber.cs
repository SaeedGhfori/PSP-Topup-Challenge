namespace PSP.ValueObjects;

public sealed record PhoneNumber
{
    public string Value { get; }

    public PhoneNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(nameof(value));

        if (!value.StartsWith("09"))
            throw new ArgumentException(nameof(value));

        if (value.Length != 11)
            throw new ArgumentException(nameof(value));

        Value = value;
    }

    public override string ToString()
        => Value;

    public static implicit operator string(PhoneNumber phone)
        => phone.Value;

    public static implicit operator PhoneNumber(string value)
        => new(value);
}
