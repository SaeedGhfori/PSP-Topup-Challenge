namespace PSP.Mock.Bank.Api.Contracts;

public sealed record ReversalResponse
(
    bool Success,
    int ResponseCode,
    string Message
);
