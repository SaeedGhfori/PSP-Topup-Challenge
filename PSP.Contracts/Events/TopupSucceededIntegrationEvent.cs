namespace PSP.Contracts.Events;

public sealed record TopupSucceededIntegrationEvent(
    Guid TransactionId,
    string ProviderReferenceNumber,
    DateTime CompletedAtUtc);
