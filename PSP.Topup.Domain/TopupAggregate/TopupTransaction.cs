using PSP.Topup.Domain.Common;
using PSP.Topup.Domain.TopupAggregate.Enums;
using PSP.Topup.Domain.TopupAggregate.Events;
using PSP.Topup.Domain.TopupAggregate.ValueObjects;

namespace PSP.Topup.Domain.TopupAggregate;

public sealed class TopupTransaction : AggregateRoot<Guid>
{
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

        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;

        Raise(new TopupRequestedDomainEvent(id));
    }

    public PhoneNumber PhoneNumber { get; }

    public Money Amount { get; }

    public MobileOperator MobileOperator { get; }

    public TransactionStatus Status { get; private set; }

    public string IdempotencyKey { get; }

    public string? ProviderReference { get; private set; }

    public string? FailureReason { get; private set; }

    public DateTime CreatedAtUtc { get; }

    public DateTime UpdatedAtUtc { get; private set; }

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
        ProviderReference = providerReference;

        Status = TransactionStatus.TopupSucceeded;

        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void MarkFailed(string reason)
    {
        FailureReason = reason;

        Status = TransactionStatus.Failed;

        UpdatedAtUtc = DateTime.UtcNow;
    }
}
