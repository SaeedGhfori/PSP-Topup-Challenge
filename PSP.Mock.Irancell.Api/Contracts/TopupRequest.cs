namespace PSP.Mock.Irancell.Api.Contracts;

public sealed record TopupRequest
(
    string MobileNumber,
    decimal Amount,
    string RequestId
);
