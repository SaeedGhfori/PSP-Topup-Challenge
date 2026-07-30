using MassTransit;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using PSP.Topup.Application.Abstractions;
using PSP.Topup.Application.Integrations;
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
        services.Configure<TopupProviderOptions>(
            configuration.GetSection(TopupProviderOptions.SectionName));

        services.AddTransient<LoggingHandler>();

        services.AddTransient<CorrelationIdHandler>();

        services.AddHttpClient<MciTopupProvider>((provider, client) =>
        {
            var options =
                provider.GetRequiredService<IOptions<TopupProviderOptions>>().Value;

            client.BaseAddress =
                new Uri(options.Mci.BaseUrl);
        })
        .AddHttpMessageHandler<CorrelationIdHandler>()
        .AddHttpMessageHandler<LoggingHandler>()
        .AddStandardResilienceHandler();

        services.AddHttpClient<IrancellTopupProvider>((provider, client) =>
        {
            var options =
                provider.GetRequiredService<IOptions<TopupProviderOptions>>().Value;

            client.BaseAddress =
                new Uri(options.Irancell.BaseUrl);
        })
        .AddHttpMessageHandler<CorrelationIdHandler>()
        .AddHttpMessageHandler<LoggingHandler>()
        .AddStandardResilienceHandler();

        services.AddScoped<ITopupProvider>(provider =>
        {
            var options =
                provider.GetRequiredService<IOptions<TopupProviderOptions>>().Value;

            return options.Provider switch
            {
                TopupProviderType.Mci => provider.GetRequiredService<MciTopupProvider>(),
                TopupProviderType.Irancell => provider.GetRequiredService<IrancellTopupProvider>(),
                _ => throw new InvalidOperationException($"Unsupported provider: {options.Provider}")
            };
        });

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
