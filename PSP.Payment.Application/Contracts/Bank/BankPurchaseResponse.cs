namespace PSP.Payment.Application.Contracts.Bank
{
    public sealed record BankPurchaseResponse(
        bool Success,
        string Rrn,
        int ResponseCode,
        string Message);
}
