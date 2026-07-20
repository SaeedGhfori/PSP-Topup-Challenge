namespace PSP.Mock.Bank.Api.Contracts.Responses;

public sealed record BalanceResponse
(
    bool Success,
    decimal Balance
);
