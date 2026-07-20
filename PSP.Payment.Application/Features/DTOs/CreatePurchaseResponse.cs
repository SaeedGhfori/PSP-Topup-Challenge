namespace PSP.Payment.Application.Features.DTOs;

public sealed record CreatePurchaseResponse(
    Guid TransactionId,
    string Status);
