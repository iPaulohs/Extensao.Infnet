using Geekhub.Worker.Workers;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<AccountCreatedEventHandler>();

var host = builder.Build();
host.Run();
