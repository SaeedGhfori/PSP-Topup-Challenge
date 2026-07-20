namespace PSP.Topup.Application.Contracts.Messaging;

public interface IEventPublisher
{
    Task PublishAsync<T>(
        T message,
        CancellationToken cancellationToken = default)
        where T : class;
}
