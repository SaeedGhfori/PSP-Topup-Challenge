namespace PSP.Topup.Infrastructure.Contracts.Responses;

public sealed record MciTopupResponse(
    bool Success,
    string Status,
    string Message,
    string? ReferenceNumber);
