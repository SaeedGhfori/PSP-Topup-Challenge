namespace PSP.Contracts.Events;


namespace PSP.Contracts.Events;

public sealed record TopupFailedIntegrationEvent(
    Guid TransactionId,
    string ErrorCode,
    string ErrorMessage,
    DateTime FailedAtUtc);
