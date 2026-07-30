namespace PSP.Topup.Application.Integrations;

public sealed record TopupRequest(
    string MobileNumber,
    decimal Amount,
    string RequestId);
