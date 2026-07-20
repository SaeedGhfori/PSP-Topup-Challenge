using PSP.Contracts.Messaging;

namespace PSP.Contracts.IntegrationEvents;

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
