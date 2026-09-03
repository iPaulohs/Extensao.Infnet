using Geekhub.Backend.Domain.Adapters;
using Geekhub.Backend.Infrastructure.Neo4JAdapter;

namespace Microsoft.Extensions.DependencyInjection;

public static class Ne4JAdapterExtensions
{
    public static IServiceCollection AddNeo4JAdapter(this IServiceCollection services, Action<Neo4JAdapterOptions> configureOptions)
    {
        var options = new Neo4JAdapterOptions();
        configureOptions(options);
        services.AddSingleton(options);
        services.AddSingleton<INeo4JAdapter, Neo4JAdapter>();
        return services;
    }
}
