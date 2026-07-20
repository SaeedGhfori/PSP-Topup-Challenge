namespace PSP.Mock.Bank.Api.Contracts;

public sealed record PurchaseResponse
(
    bool Success,
    int ResponseCode,
    string Message,
    string? Rrn
);
