namespace PSP.Mock.Bank.Api.Contracts.Responses;

public sealed record ConfirmationResponse
(
    bool Success,
    int ResponseCode,
    string Message
);
