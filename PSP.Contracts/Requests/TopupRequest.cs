namespace PSP.Contracts.Requests;

public sealed record TopupRequest(
    string MobileNumber,
    decimal Amount);
