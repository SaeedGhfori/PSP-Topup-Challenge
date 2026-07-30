namespace PSP.Mock.Irancell.Api.Contracts;

public sealed record TopupResponse
(
    bool Success,
    string Status,
    string Message,
    string? ReferenceNumber
);
