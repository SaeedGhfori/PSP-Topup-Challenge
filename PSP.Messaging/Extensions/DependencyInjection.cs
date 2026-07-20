using MassTransit;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using PSP.Messaging.Abstractions;

namespace PSP.Messaging;

public static class DependencyInjection
{
    public static IServiceCollection AddMessaging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var options =
            configuration.GetSection(RabbitMqOptions.SectionName)
                .Get<RabbitMqOptions>()!;

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(
                    options.Host,
                    "/",
                    h =>
                    {
                        h.Username(options.Username);
                        h.Password(options.Password);
                    });
            });
        });

        services.AddScoped<IMessageBus, MassTransitMessageBus>();

        return services;
    }
}
