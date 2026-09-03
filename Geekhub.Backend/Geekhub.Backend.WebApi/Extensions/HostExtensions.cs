using Geekhub.Backend.Application;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.Postgresql;
using Wolverine.RabbitMQ;

namespace Geekhub.Backend.WebApi.Extensions
{
    public static class HostExtensions
    {
        public static WebApplicationBuilder AddHostExtensions(this WebApplicationBuilder builder)
        {
            builder.UseWolverine(opts =>
            {
                opts.Discovery
                    .IncludeAssembly(typeof(ApplicationAssemblyReference).Assembly);

                var connectionString =
                   builder.Configuration["DatabaseAdapterOptions:ConnectionString"]!;

                opts.PersistMessagesWithPostgresql(
                    connectionString,
                    schemaName: "outbox");

                opts.UseRabbitMq(new Uri(builder.Configuration["RabbitMQAdapterOptions:BootstrapServers"]!))
                .UseConventionalRouting(x =>
                {
                    x.QueueNameForListener(type => $"{type.Name}Queue");
                })
                .AutoProvision();

                opts.UseEntityFrameworkCoreTransactions();

                opts.Policies.AutoApplyTransactions();
            });

            return builder;
        }
    }
}


