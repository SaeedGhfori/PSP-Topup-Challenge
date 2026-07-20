namespace PSP.Events;

public sealed record TopupFailedIntegrationEvent(
    Guid TransactionId,
    string ErrorCode,
    string ErrorMessage,
    DateTime FailedAtUtc);
