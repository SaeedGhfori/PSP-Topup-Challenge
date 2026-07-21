using MassTransit;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using PSP.Payment.Application.Contracts.Bank;
using PSP.Payment.Infrastructure.Clients;
using PSP.Payment.Infrastructure.Consumers;

namespace PSP.Payment.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient<IBankClient, BankClient>(client =>
        {
            client.BaseAddress =
                new Uri(configuration["Bank:BaseUrl"]!);
        });

        services.AddMassTransit(x =>
        {
            x.AddConsumer<TopupCompletedConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddScoped<PSP.Messaging.Abstractions.IMessageBus, MassTransitMessageBus>();

        return services;
    }
}
