using PSP.Events;
using PSP.Topup.Domain.Common;
using PSP.Topup.Domain.Enums;
using PSP.Topup.Domain.Events;
using PSP.Topup.Domain.ValueObjects;

namespace PSP.Topup.Domain.Entities;

/// <summary>
/// Represents a top-up transaction aggregate.
/// </summary>
public sealed class TopupTransaction : AuditableEntity<Guid>
{
    // Required by EF Core
    private TopupTransaction()
        : base(Guid.Empty)
    {
    }

    private TopupTransaction(
        Guid id,
        PhoneNumber phoneNumber,
        Money amount,
        MobileOperator mobileOperator,
        string idempotencyKey)
        : base(id)
    {
        PhoneNumber = phoneNumber;
        Amount = amount;
        MobileOperator = mobileOperator;
        IdempotencyKey = idempotencyKey;

        Status = TransactionStatus.Pending;

        RaiseDomainEvent(new TopupRequestedDomainEvent(id));
    }

    public PhoneNumber PhoneNumber { get; private set; } = default!;

    public Money Amount { get; private set; } = default!;

    public MobileOperator MobileOperator { get; private set; }

    public TransactionStatus Status { get; private set; }

    public string IdempotencyKey { get; private set; } = string.Empty;

    /// <summary>
    /// Reference returned by MCI after successful top-up.
    /// </summary>
    public string? ProviderReference { get; private set; }

    /// <summary>
    /// Failure reason returned by provider.
    /// </summary>
    public string? FailureReason { get; private set; }

    public static TopupTransaction Create(
        PhoneNumber phoneNumber,
        Money amount,
        MobileOperator mobileOperator,
        string idempotencyKey)
    {
        return new TopupTransaction(
            Guid.NewGuid(),
            phoneNumber,
            amount,
            mobileOperator,
            idempotencyKey);
    }

    public void MarkSucceeded(string providerReference)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(providerReference);

        ProviderReference = providerReference;
        FailureReason = null;
        Status = TransactionStatus.TopupSucceeded;

        MarkUpdated();
    }

    public void MarkFailed(string reason)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reason);

        FailureReason = reason;
        Status = TransactionStatus.Failed;

        MarkUpdated();
    }

    public void MarkConfirmationSent()
    {
        Status = TransactionStatus.ConfirmationSent;

        MarkUpdated();
    }

    public void MarkReversed(string reason)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reason);

        FailureReason = reason;
        Status = TransactionStatus.ReversalSent;

        MarkUpdated();
    }
}
