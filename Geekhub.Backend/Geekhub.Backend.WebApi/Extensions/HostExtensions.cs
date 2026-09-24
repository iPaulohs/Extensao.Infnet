using Geekhub.Backend.Application;
using Geekhub.Backend.Domain;
using Geekhub.Backend.Domain.Events;
using System.Text.RegularExpressions;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.Kafka;
using Wolverine.Postgresql;

namespace Geekhub.Backend.WebApi.Extensions
{
    public static partial class HostExtensions
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

                opts.UseKafka(builder.Configuration["KafkaAdapterOptions:BootstrapServers"]!)
                    .AutoProvision();

                var eventTypes = typeof(DomainAssemblyReference).Assembly
                    .GetTypes()
                    .Where(t => typeof(IEvent).IsAssignableFrom(t)
                     && !t.IsInterface
                     && !t.IsAbstract
                     && !t.IsGenericType);

                foreach (var eventType in eventTypes)
                {
                    var topicName = ToKafkaTopicName(eventType.Name);

                    opts.PublishMessage(eventType)
                        .ToKafkaTopic(topicName);
                }

                static string ToKafkaTopicName(string typeName)
                {
                    if (typeName.EndsWith("Event", StringComparison.OrdinalIgnoreCase))
                    {
                        typeName = typeName[..^5];
                    }

                    return EventNameRegex()
                    .Replace(typeName, "$1-$2")
                    .ToLowerInvariant();
                }

                opts.UseEntityFrameworkCoreTransactions();

                opts.Policies.AutoApplyTransactions();
            });

            return builder;
        }

        [GeneratedRegex(@"([a-z0-9])([A-Z])")]
        private static partial Regex EventNameRegex();
    }
}


