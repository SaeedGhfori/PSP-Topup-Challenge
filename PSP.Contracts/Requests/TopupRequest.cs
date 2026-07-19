namespace PSP.Contracts.Events;

public sealed record TopupRequest(
    string MobileNumber,
    decimal Amount);
