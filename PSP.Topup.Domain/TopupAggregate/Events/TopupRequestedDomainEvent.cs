using PSP.Topup.Domain.Common;

namespace PSP.Topup.Domain.TopupAggregate.Events;

/// <summary>
/// Raised when a topup transaction is created.
/// </summary>
public sealed class TopupRequestedDomainEvent : IDomainEvent
{
    public TopupRequestedDomainEvent(Guid transactionId)
    {
        TransactionId = transactionId;
        OccurredOnUtc = DateTime.UtcNow;
    }

    public Guid TransactionId { get; }

    public DateTime OccurredOnUtc { get; }
}
