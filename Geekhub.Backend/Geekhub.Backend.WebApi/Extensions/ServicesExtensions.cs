using FluentValidation;
using Geekhub.Backend.Application;
using Geekhub.Backend.Domain;
using Geekhub.Backend.Infrastructure.RedisAdapter.Microsoft.Extensions.DependencyInjection;
using Mapster;

namespace Geekhub.Backend.WebApi.Extensions;

public static class ServicesExtensions
{
    public static void AddExtensionsServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOpenApi()
            .AddValidatorsFromAssemblyContaining<DomainAssemblyReference>()
            .AddMapster();

        TypeAdapterConfig
            .GlobalSettings
            .Scan(typeof(ApplicationAssemblyReference).Assembly);

        services
            .AddMinIoAdapter(config =>
            {
                config.Endpoint = configuration
                    .GetSection("MinIoAdapterOptions:Endpoint").Value!;
            })
            .AddDatabaseAdapter(config =>
            {
                config.ConnectionString = configuration
                    .GetSection("DatabaseAdapterOptions:ConnectionString").Value!;
            })
            .AddTmdbAdapter(config =>
            {
                config.ApiKey = configuration
                    .GetSection("TmdbAdapterOptions:ApiKey").Value!;
            })
            .AddRabbitMQAdapter(config =>
            {
                config.BootstrapServers = configuration
                    .GetSection("RabbitMQAdapterOptions:BootstrapServers").Value!;
            })
            .AddRedisAdapter(config =>
            {
                config.Configuration = configuration
                    .GetSection("RedisAdapterOptions:Configuration").Value!;
            })
            .AddNeo4JAdapter(config =>
            {
                config.ConnectionString = configuration
                    .GetSection("Neo4JAdapterOptions:ConnectionString").Value!;
            });
    }
}
