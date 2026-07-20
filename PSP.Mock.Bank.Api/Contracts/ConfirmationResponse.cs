namespace PSP.Mock.Bank.Api.Contracts;

public sealed record ConfirmationResponse
(
    bool Success,
    int ResponseCode,
    string Message
);
