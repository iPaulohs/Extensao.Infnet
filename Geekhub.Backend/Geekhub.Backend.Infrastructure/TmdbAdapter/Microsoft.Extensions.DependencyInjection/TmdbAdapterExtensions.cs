using TMDbLib.Client;

namespace Microsoft.Extensions.DependencyInjection;

public static class TmdbAdapterExtensions
{
    public static IServiceCollection AddTmdbAdapter(this IServiceCollection services, Action<TmdbAdapterOptions> configureOptions)
    {
        var options = new TmdbAdapterOptions();
        configureOptions(options);
        services.AddSingleton(options);

        services.AddSingleton<TMDbClient>(sp =>
        {
            return new(options.ApiKey);
        });

        return services;
    }
}
