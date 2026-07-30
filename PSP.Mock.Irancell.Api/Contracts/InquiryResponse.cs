namespace PSP.Mock.Irancell.Api.Contracts;

public sealed record InquiryResponse
(
    string ReferenceNumber,
    string Status,
    string Message
);
