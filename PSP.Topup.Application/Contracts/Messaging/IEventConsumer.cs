namespace PSP.Topup.Application.Contracts.Messaging;

public interface IEventConsumer<T>
{
    Task ConsumeAsync(
        T message,
        CancellationToken cancellationToken);
}
