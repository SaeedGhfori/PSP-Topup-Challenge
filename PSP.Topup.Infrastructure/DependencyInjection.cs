using MassTransit;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using PSP.Messaging;
using PSP.Topup.Application.Contracts.Services;
using PSP.Topup.Application.Contracts.Services.Mci;
using PSP.Topup.Infrastructure.Clients;
using PSP.Topup.Infrastructure.Configuration;
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
                var rabbitOptions = new RabbitMqOptions();

                configuration
                    .GetSection(RabbitMqOptions.SectionName)
                    .Bind(rabbitOptions);

                cfg.Host(
                    rabbitOptions.Host,
                    "/",
                    h =>
                    {
                        h.Username(rabbitOptions.Username);
                        h.Password(rabbitOptions.Password);
                    });

                cfg.ReceiveEndpoint(
                    "topup-requested-queue",
                    e =>
                    {
                        e.ConfigureConsumer<TopupRequestedConsumer>(context);
                    });
            });
        });


        return services;
    }
}
