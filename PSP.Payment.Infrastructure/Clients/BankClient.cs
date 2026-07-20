using PSP.Payment.Application.Contracts.Bank;

namespace PSP.Payment.Infrastructure.Clients;

public sealed class BankClient : IBankClient
{
    public Task<BankPurchaseResponse> PurchaseAsync(
        BankPurchaseRequest request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task ConfirmationAsync(
        BankConfirmationRequest request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task ReversalAsync(
        BankReversalRequest request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
