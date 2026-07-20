using PSP.Contracts.Enums;

namespace PSP.Contracts.Responses;

public sealed record TopupResponse(
    Guid TransactionId,
    TopupStatus Status,
    string? Message);
