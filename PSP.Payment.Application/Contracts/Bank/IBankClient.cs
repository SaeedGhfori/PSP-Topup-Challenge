namespace PSP.Payment.Application.Contracts.Bank;

public interface IBankClient
{
    Task<BankPurchaseResponse> PurchaseAsync(
        BankPurchaseRequest request,
        CancellationToken cancellationToken);

    Task ConfirmationAsync(
        BankConfirmationRequest request,
        CancellationToken cancellationToken);

    Task ReversalAsync(
        BankReversalRequest request,
        CancellationToken cancellationToken);
}
