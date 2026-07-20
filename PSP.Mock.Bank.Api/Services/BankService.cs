using PSP.Mock.Bank.Api.Contracts.Requests;
using PSP.Mock.Bank.Api.Contracts.Responses;
using PSP.Mock.Bank.Api.Enums;
using PSP.Mock.Bank.Api.Helpers;

namespace PSP.Mock.Bank.Api.Services;

public sealed class BankService : IBankService
{
    public async Task<PurchaseResponse> PurchaseAsync(PurchaseRequest request)
    {
        await Task.Delay(Random.Shared.Next(300, 700));

        if (request.Pan.EndsWith("9999"))
        {
            throw new TimeoutException("Bank timeout.");
        }

        if (request.Pan.EndsWith("8888"))
        {
            return new PurchaseResponse(
                false,
                (int)ResponseCode.CardBlocked,
                "Card is blocked.",
                null);
        }

        if (request.Pan.EndsWith("7777"))
        {
            return new PurchaseResponse(
                false,
                (int)ResponseCode.InsufficientFunds,
                "Insufficient funds.",
                null);
        }

        if (request.Pan.EndsWith("6666"))
        {
            return new PurchaseResponse(
                false,
                (int)ResponseCode.Duplicate,
                "Duplicate transaction.",
                null);
        }

        if (request.Pan.EndsWith("5555"))
        {
            return new PurchaseResponse(
                false,
                (int)ResponseCode.InternalError,
                "Internal bank error.",
                null);
        }

        return new PurchaseResponse(
            true,
            (int)ResponseCode.Success,
            "Purchase successful.",
            RrnGenerator.Generate());
    }

    public async Task<ConfirmationResponse> ConfirmationAsync(ConfirmationRequest request)
    {
        await Task.Delay(200);

        return new ConfirmationResponse(
            true,
            (int)ResponseCode.Success,
            "Confirmation completed.");
    }

    public async Task<ReversalResponse> ReversalAsync(ReversalRequest request)
    {
        await Task.Delay(200);

        return new ReversalResponse(
            true,
            (int)ResponseCode.Success,
            "Reversal completed.");
    }

    public async Task<BalanceResponse> BalanceAsync(BalanceRequest request)
    {
        await Task.Delay(150);

        return new BalanceResponse(
            true,
            15_000_000);
    }
}
