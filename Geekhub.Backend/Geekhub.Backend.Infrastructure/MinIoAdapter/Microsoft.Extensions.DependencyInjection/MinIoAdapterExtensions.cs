using Geekhub.Backend.Domain.Adapters;
using Geekhub.Backend.Infrastructure.MinIoAdapter;

namespace Microsoft.Extensions.DependencyInjection;

public static class MinIoAdapterExtensions
{
    public static IServiceCollection AddMinIoAdapter(this IServiceCollection services, Action<MinIoAdapterOptions> configure)
    {
        services.AddScoped<IMinioAdapter, MinioAdapter>();
        return services;
    }
}
