namespace PSP.Mock.Bank.Api.Contracts.Requests;

public sealed record PurchaseRequest
(
    string Pan,
    decimal Amount,
    string TerminalId,
    string TraceNumber
);
