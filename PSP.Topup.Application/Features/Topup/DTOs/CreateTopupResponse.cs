namespace PSP.Topup.Application.Features.Topup.Create;

public sealed record CreateTopupResponse(
    Guid TransactionId,
    string Status);
