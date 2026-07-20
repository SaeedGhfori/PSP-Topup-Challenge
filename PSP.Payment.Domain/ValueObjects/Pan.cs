namespace PSP.Payment.Domain.ValueObjects;

public sealed record Pan
{
    public string Value { get; }

    private Pan(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(nameof(value));

        if (value.Length != 16)
            throw new ArgumentException(nameof(value));

        Value = value;
    }

    public static Pan Create(string value)
        => new(value);

    public static implicit operator string(Pan value)
        => value.Value;
}
