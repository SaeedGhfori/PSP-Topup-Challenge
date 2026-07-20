namespace PSP.Topup.Application.Contracts.Messaging;

public interface IMessageBus
{
    Task PublishAsync<T>(
        T message,
        CancellationToken cancellationToken = default);
}
