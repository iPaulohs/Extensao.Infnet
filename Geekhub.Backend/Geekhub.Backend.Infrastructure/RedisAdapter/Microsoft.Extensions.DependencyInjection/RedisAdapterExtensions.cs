using Geekhub.Backend.Domain.Adapters;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Geekhub.Backend.Infrastructure.RedisAdapter.Microsoft.Extensions.DependencyInjection
{
    public static class RedisAdapterExtensions
    {
        public static IServiceCollection AddRedisAdapter(this IServiceCollection services, Action<RedisAdapterOptions> configureOptions)
        {
            var options = new RedisAdapterOptions();
            configureOptions(options);
            services.AddSingleton(options);

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                return ConnectionMultiplexer.Connect(options.Configuration);
            });

            services.AddScoped<IRedisAdapter, RedisAdapter>();

            return services;
        }
    }
}
