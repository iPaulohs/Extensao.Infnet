using FluentValidation;
using Geekhub.Backend.Application;
using Geekhub.Backend.Domain;
using Geekhub.Backend.Domain.Models;
using Geekhub.Backend.Infrastructure.RedisAdapter.Microsoft.Extensions.DependencyInjection;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
            .AddRedisAdapter(config =>
            {
                config.Configuration = configuration
                    .GetSection("RedisAdapterOptions:Configuration").Value!;
            })
            .AddNeo4JAdapter(config =>
            {
                config.ConnectionString = configuration
                    .GetSection("Neo4JAdapterOptions:ConnectionString").Value!;
            })
            .AddApplication();

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization();

        services.Configure<JwtSettings>(
            configuration.GetSection(JwtSettings.SectionName));
    }
}
