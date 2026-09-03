using Geekhub.Backend.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddHostExtensions();

builder.Services.AddExtensionsServices(builder.Configuration);

var app = builder.Build();

app.AddBuilderExtensions();

app.Run();
