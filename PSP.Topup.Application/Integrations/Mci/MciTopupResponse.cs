namespace PSP.Topup.Application.Integrations.Mci;

public sealed record MciTopupResponse(
    bool Success,
    string Status,
    string Message,
    string? ReferenceNumber);
