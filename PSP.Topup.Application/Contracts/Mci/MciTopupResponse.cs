namespace PSP.Topup.Application.Contracts.Mci;

public sealed record MciTopupResponse(
    bool Success,
    string Status,
    string Message,
    string? ReferenceNumber);
