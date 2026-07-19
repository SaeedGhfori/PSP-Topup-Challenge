using PSP.Enums;

namespace PSP.Responses;

public sealed record TopupResponse(
    Guid TransactionId,
    TopupStatus Status,
    string? Message);
