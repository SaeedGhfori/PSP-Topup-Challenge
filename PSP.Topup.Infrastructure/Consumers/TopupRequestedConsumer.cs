using MassTransit;

using Microsoft.Extensions.Logging;

using PSP.Contracts.Events;
using PSP.Topup.Application.Contracts.Services;

namespace PSP.Topup.Infrastructure.Consumers;

public sealed class TopupRequestedConsumer
    : IConsumer<TopupRequestedIntegrationEvent>
{
    private readonly ITopupProcessor _processor;
    private readonly ILogger<TopupRequestedConsumer> _logger;

    public TopupRequestedConsumer(
        ITopupProcessor processor,
        ILogger<TopupRequestedConsumer> logger)
    {
        _processor = processor;
        _logger = logger;
    }

    public async Task Consume(
        ConsumeContext<TopupRequestedIntegrationEvent> context)
    {
        _logger.LogInformation(
            "Received TopupRequestedIntegrationEvent {TransactionId}",
            context.Message.TransactionId);

        await _processor.ProcessAsync(
            context.Message.TransactionId,
            context.CancellationToken);
    }
}
