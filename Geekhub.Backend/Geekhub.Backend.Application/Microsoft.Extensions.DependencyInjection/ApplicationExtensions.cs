using Geekhub.Backend.Application.Services;
using Geekhub.Backend.Domain.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IJwtHandlerService, JwtHandlerService>();

        return services;
    }
}
