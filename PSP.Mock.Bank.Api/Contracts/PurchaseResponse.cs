namespace PSP.Mock.Bank.Api.Contracts.Responses;

public sealed record PurchaseResponse
(
    bool Success,
    int ResponseCode,
    string Message,
    string? Rrn
);
