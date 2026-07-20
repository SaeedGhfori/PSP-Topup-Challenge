using FluentValidation;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using PSP.Topup.Application.Behaviors.Logging;
using PSP.Topup.Application.Behaviors.Validation;

namespace PSP.Topup.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));


        return services;
    }
}
