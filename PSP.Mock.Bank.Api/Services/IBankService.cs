using PSP.Mock.Bank.Api.Contracts.Requests;
using PSP.Mock.Bank.Api.Contracts.Responses;

namespace PSP.Mock.Bank.Api.Services;

public interface IBankService
{
    Task<PurchaseResponse> PurchaseAsync(PurchaseRequest request);

    Task<ConfirmationResponse> ConfirmationAsync(ConfirmationRequest request);

    Task<ReversalResponse> ReversalAsync(ReversalRequest request);

    Task<BalanceResponse> BalanceAsync(BalanceRequest request);
}
