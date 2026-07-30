namespace PSP.Topup.Application.Integrations;

public sealed record TopupResponse(
    bool Success,
    string Status,
    string Message,
    string? ReferenceNumber);
