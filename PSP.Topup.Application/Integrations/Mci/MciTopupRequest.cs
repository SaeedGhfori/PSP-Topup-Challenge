namespace PSP.Topup.Application.Integrations.Mci;

public sealed record MciTopupRequest(
    string MobileNumber,
    decimal Amount,
    string RequestId);
