
using PSP.Topup.Domain.Enums;

namespace PSP.Topup.Domain.Entities;

public sealed class TopupTransaction
{
    public Guid Id { get; private set; }

    public string PhoneNumber { get; private set; } = null!;

    public decimal Amount { get; private set; }

    public int OperatorId { get; private set; }

    public string IdempotencyKey { get; private set; } = null!;

    public TopupStatus Status { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    private TopupTransaction()
    {
    }

    public TopupTransaction(
        string phoneNumber,
        decimal amount,
        int operatorId,
        string idempotencyKey)
    {
        Id = Guid.NewGuid();
        PhoneNumber = phoneNumber;
        Amount = amount;
        OperatorId = operatorId;
        IdempotencyKey = idempotencyKey;

        Status = TopupStatus.Pending;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void MarkSucceeded()
    {
        Status = TopupStatus.Succeeded;
    }

    public void MarkFailed()
    {
        Status = TopupStatus.Failed;
    }

    public void MarkReversed()
    {
        Status = TopupStatus.Reversed;
    }
}
