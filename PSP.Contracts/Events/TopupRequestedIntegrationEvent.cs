namespace PSP.Contracts.Events;

public sealed record TopupRequestedIntegrationEvent(
    Guid TransactionId,
    string MobileNumber,
    decimal Amount,
    DateTime CreatedAtUtc);
