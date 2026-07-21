using MassTransit;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using PSP.Topup.Application.Abstractions;
using PSP.Topup.Application.Integrations.Mci;
using PSP.Topup.Infrastructure.Clients;
using PSP.Topup.Infrastructure.Messaging.Consumers;
using PSP.Topup.Infrastructure.Options;
using PSP.Topup.Infrastructure.Services;

namespace PSP.Topup.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MciOptions>(
            configuration.GetSection(MciOptions.SectionName));

        services.AddTransient<LoggingHandler>();

        services.AddTransient<CorrelationIdHandler>();

        services.AddHttpClient<IMciClient, MciClient>((provider, client) =>
        {
            var options =
                provider.GetRequiredService<IOptions<MciOptions>>().Value;

            client.BaseAddress =
                new Uri(options.BaseUrl);

        })
        .AddHttpMessageHandler<CorrelationIdHandler>()
        .AddHttpMessageHandler<LoggingHandler>()
        .AddStandardResilienceHandler();

        services.AddScoped<ITopupProcessor, TopupProcessor>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<TopupRequestedConsumer>();

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
