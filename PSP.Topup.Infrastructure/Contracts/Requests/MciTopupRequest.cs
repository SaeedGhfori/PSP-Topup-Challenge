namespace PSP.Topup.Infrastructure.Contracts.Requests;

public sealed record MciTopupRequest(
    string MobileNumber,
    decimal Amount,
    string RequestId);
