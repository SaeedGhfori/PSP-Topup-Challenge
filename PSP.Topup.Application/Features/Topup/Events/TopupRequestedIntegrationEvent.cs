public sealed record TopupRequestedIntegrationEvent(
    Guid TransactionId,
    string PhoneNumber,
    decimal Amount,
    int OperatorId,
    string IdempotencyKey);
