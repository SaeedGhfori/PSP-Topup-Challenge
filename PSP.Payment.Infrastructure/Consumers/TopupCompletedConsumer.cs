using MassTransit;

using Microsoft.Extensions.Logging;

using PSP.Contracts.Events;
using PSP.Payment.Application.Contracts.Bank;

namespace PSP.Payment.Infrastructure.Consumers;

public sealed class TopupCompletedConsumer
    : IConsumer<TopupCompletedIntegrationEvent>
{
    private readonly IBankClient _bankClient;
    private readonly ILogger<TopupCompletedConsumer> _logger;

    public TopupCompletedConsumer(
        IBankClient bankClient,
        ILogger<TopupCompletedConsumer> logger)
    {
        _bankClient = bankClient;
        _logger = logger;
    }

    public async Task Consume(
        ConsumeContext<TopupCompletedIntegrationEvent> context)
    {
        var message = context.Message;

        if (message.Success)
        {
            await _bankClient.ConfirmationAsync(
                new BankConfirmationRequest(
                    message.ProviderReference!),
                context.CancellationToken);

            _logger.LogInformation(
                "Bank confirmation sent.");
        }
        else
        {
            await _bankClient.ReversalAsync(
                new BankReversalRequest(
                    message.ProviderReference!),
                context.CancellationToken);

            _logger.LogInformation(
                "Bank reversal sent.");
        }
    }
}
