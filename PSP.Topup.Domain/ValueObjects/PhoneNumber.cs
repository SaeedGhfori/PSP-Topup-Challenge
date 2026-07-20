public sealed record PhoneNumber
{
    public string Value { get; }

    private PhoneNumber(string value)
    {
        // Validation
        Value = value;
    }

    public static PhoneNumber Create(string value)
        => new(value);

    public static implicit operator string(PhoneNumber phone)
        => phone.Value;
}
