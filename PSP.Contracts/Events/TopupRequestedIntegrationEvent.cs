using PSP.Messaging.Consumers;

namespace PSP.Contracts.Events;

public sealed record TopupRequestedIntegrationEvent(
    Guid TransactionId,
    string PhoneNumber,
    decimal Amount,
    int OperatorId,
    string IdempotencyKey)
    : IIntegrationEvent
{
    public Guid EventId { get; init; }
        = Guid.NewGuid();

    public DateTime OccurredOnUtc { get; init; }
        = DateTime.UtcNow;
}
