namespace PSP.Payment.Application.Features.Events;

public sealed record TopupRequestedIntegrationEvent(
    Guid TransactionId,
    string PhoneNumber,
    decimal Amount,
    int OperatorId,
    string IdempotencyKey);
