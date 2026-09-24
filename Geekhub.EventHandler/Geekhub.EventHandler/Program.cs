using Confluent.Kafka;
using Geekhub.EventHandler.Infrastructure.EmailAdapter.Microsoft.Extensions.DependencyInjection;
using Geekhub.EventHandler.Messaging;
using Wolverine;
using Wolverine.Kafka;
using Wolverine.Util;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddEmailAdapter(options =>
    builder.Configuration.GetSection(EmailAdapterOptions.SectionName).Bind(options));
var bootstrapServers = builder.Configuration["KafkaAdapterOptions:BootstrapServers"];
var groupId = builder.Configuration["KafkaAdapterOptions:GroupId"];

if (string.IsNullOrWhiteSpace(bootstrapServers) || string.IsNullOrWhiteSpace(groupId))
{
    throw new InvalidOperationException(
        "Configure KafkaAdapterOptions:BootstrapServers e KafkaAdapterOptions:GroupId.");
}

builder.UseWolverine(options =>
{
    WolverineMessageNaming.InsertFirst<IncomingEventNaming>();

    options.UseKafka(bootstrapServers)
        .ConfigureConsumers(consumer =>
        {
            consumer.GroupId = groupId;
            consumer.AutoOffsetReset = AutoOffsetReset.Earliest;
        })
        .AutoProvision();

    options.ListenToKafkaTopic("account-created")
        .ProcessInline();
});

var host = builder.Build();
await host.RunAsync();
