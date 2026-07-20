namespace PSP.Mock.Bank.Api.Contracts.Responses;

public sealed record ReversalResponse
(
    bool Success,
    int ResponseCode,
    string Message
);
