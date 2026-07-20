namespace PSP.Requests;

public sealed record TopupRequest(
    string MobileNumber,
    decimal Amount);
