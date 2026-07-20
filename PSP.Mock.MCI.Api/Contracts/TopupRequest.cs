namespace PSP.Mock.MCI.Api.Contracts;

public sealed record TopupRequest
(
    string MobileNumber,
    decimal Amount,
    string RequestId
);
