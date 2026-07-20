using PSP.Payment.Application.Contracts.Messaging;

namespace PSP.Payment.Infrastructure.Messaging;

public sealed class RabbitMqMessageBus : IMessageBus
{
    public Task PublishAsync<T>(
        T message,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
