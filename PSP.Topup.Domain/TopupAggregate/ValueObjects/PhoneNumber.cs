using PSP.Topup.Domain.Common;

namespace PSP.Topup.Domain.TopupAggregate.ValueObjects;

/// <summary>
/// Represents a validated mobile phone number.
/// </summary>
public sealed class PhoneNumber : ValueObject
{
    public string Value { get; }

    public PhoneNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Phone number is required.", nameof(value));

        value = value.Trim();

        if (!value.StartsWith("09") || value.Length != 11)
            throw new ArgumentException("Phone number format is invalid.", nameof(value));

        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
        => Value;

    public static implicit operator string(PhoneNumber phoneNumber)
        => phoneNumber.Value;
}
