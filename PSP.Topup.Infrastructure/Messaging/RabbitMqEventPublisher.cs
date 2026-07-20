using Microsoft.Extensions.Logging;

using PSP.Topup.Application.Contracts.Messaging;

namespace PSP.Topup.Infrastructure.Messaging;

public sealed class RabbitMqEventPublisher
    : IEventPublisher
{
    private readonly ILogger<RabbitMqEventPublisher> _logger;

    public RabbitMqEventPublisher(
        ILogger<RabbitMqEventPublisher> logger)
    {
        _logger = logger;
    }

    public Task PublishAsync<T>(
        T message,
        CancellationToken cancellationToken = default)
        where T : class
    {
        _logger.LogInformation(
            "Publishing Integration Event {Event}",
            typeof(T).Name);

        return Task.CompletedTask;
    }
}
