namespace PSP.Mock.MCI.Api.Contracts;

public sealed record InquiryResponse
(
    string ReferenceNumber,
    string Status,
    string Message
);
