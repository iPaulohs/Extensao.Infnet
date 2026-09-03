namespace Microsoft.Extensions.DependencyInjection
{
    public static class RabbitMQAdapterExtensions
    {
        public static IServiceCollection AddRabbitMQAdapter(this IServiceCollection services, Action<RabbitMQAdapterOptions> configure)
        {
            return services;
        }
    }
}
