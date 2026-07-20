namespace PSP.Features.Topup.DTOs;

public sealed record CreateTopupResponse(
    Guid TransactionId,
    string Status);
