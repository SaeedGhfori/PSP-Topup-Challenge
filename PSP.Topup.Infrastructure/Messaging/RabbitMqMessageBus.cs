using MassTransit;

using PSP.Topup.Application.Contracts.Messaging;

public sealed class RabbitMqMessageBus
    : IMessageBus
{
    private readonly IPublishEndpoint _publisher;

    public RabbitMqMessageBus(
        IPublishEndpoint publisher)
    {
        _publisher = publisher;
    }

    public Task PublishAsync<T>(
        T message,
        CancellationToken cancellationToken = default)
    {
        return _publisher.Publish(
            message,
            cancellationToken);
    }
}
