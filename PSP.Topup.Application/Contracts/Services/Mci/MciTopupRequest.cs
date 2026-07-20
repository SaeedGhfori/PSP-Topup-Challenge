namespace PSP.Topup.Application.Contracts.Services.Mci;

public sealed record MciTopupRequest(
    string MobileNumber,
    decimal Amount,
    string RequestId);
