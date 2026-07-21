using Microsoft.Extensions.Logging;

using PSP.Payment.Application.Contracts.Bank;

namespace PSP.Payment.Infrastructure.Clients;

public sealed class BankClient : IBankClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<BankClient> _logger;

    public BankClient(
        HttpClient httpClient,
        ILogger<BankClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<BankPurchaseResponse> PurchaseAsync(
        BankPurchaseRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Mock Purchase => Amount:{Amount}",
            request.Amount);

        await Task.Delay(300, cancellationToken);

        return new BankPurchaseResponse(
            true,
            Guid.NewGuid().ToString("N"),
            0,
            "Purchase Successful");
    }

    public async Task ConfirmationAsync(
        BankConfirmationRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Mock Confirmation => {Rrn}",
            request.Rrn);

        await Task.Delay(300, cancellationToken);
    }

    public async Task ReversalAsync(
        BankReversalRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Mock Reversal => {Rrn}",
            request.Rrn);

        await Task.Delay(300, cancellationToken);
    }
}
