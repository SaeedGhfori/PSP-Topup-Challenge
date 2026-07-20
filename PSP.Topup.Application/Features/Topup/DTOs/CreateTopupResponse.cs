namespace PSP.Topup.Application.DTOs;

/// <summary>
/// Response returned after creating a top-up.
/// </summary>
public sealed record CreateTopupResponse(
    Guid TransactionId,
    string Status);
