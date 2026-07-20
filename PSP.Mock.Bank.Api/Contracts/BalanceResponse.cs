namespace PSP.Mock.Bank.Api.Contracts;

public sealed record BalanceResponse
(
    bool Success,
    decimal Balance
);
