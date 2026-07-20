using MassTransit;

using PSP.Messaging.Abstractions;

public sealed class MassTransitMessageBus : IMessageBus
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitMessageBus(
        IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishAsync<T>(
        T message,
        CancellationToken cancellationToken = default)
        where T : class
    {
        await _publishEndpoint.Publish(
            message,
            cancellationToken);
    }
}
