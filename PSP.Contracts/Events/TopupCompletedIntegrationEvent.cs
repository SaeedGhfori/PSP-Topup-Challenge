using PSP.Messaging;

namespace PSP.Events;

public sealed record TopupCompletedIntegrationEvent(
    Guid TransactionId,
    bool Success,
    string? ProviderReference,
    string? FailureReason)
    : IIntegrationEvent
{
    public Guid EventId { get; init; }
        = Guid.NewGuid();

    public DateTime OccurredOnUtc { get; init; }
        = DateTime.UtcNow;
}
