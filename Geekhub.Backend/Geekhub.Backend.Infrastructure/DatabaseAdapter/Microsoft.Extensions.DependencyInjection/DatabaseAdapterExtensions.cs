using Geekhub.Backend.Infrastructure.DatabaseAdapter;
using Microsoft.EntityFrameworkCore;
using Wolverine.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class DatabaseAdapterExtensions
{
    public static IServiceCollection AddDatabaseAdapter(this IServiceCollection services, Action<DatabaseAdapterOptions> configureOptions)
    {
        var options = new DatabaseAdapterOptions();
        configureOptions(options);
        services.AddSingleton(options);

        services.AddDbContextWithWolverineIntegration<AppDbContext>(opt =>
        {
            opt.UseNpgsql(options.ConnectionString);
        });

        return services;
    }
}
