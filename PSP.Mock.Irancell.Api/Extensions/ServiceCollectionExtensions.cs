using PSP.Mock.Irancell.Api.Services;

namespace PSP.Mock.Irancell.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIrancellServices(this IServiceCollection services)
    {
        services.AddSingleton<IIrancellService, IrancellService>();

        return services;
    }
}
