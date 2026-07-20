using MassTransit;

using PSP.Contracts.Events;
using PSP.Topup.Application.Contracts.Services;

public sealed class TopupRequestedConsumer
    : IConsumer<TopupRequestedIntegrationEvent>
{
    private readonly ITopupProcessor _processor;

    public TopupRequestedConsumer(
        ITopupProcessor processor)
    {
        _processor = processor;
    }

    public async Task Consume(
        ConsumeContext<TopupRequestedIntegrationEvent> context)
    {
        await _processor.ProcessAsync(
            context.Message.TransactionId,
            context.CancellationToken);
    }
}
