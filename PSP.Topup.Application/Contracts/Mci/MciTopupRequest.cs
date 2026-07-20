namespace PSP.Topup.Application.Contracts.Mci;

public sealed record MciTopupRequest(
    string MobileNumber,
    decimal Amount,
    string RequestId);
