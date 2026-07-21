namespace PSP.Payment.Application.Features.Payments.DTOs;

public sealed record CreatePurchaseResponse(
    Guid TransactionId,
    string Status);
