using Geekhub.EventHandler.Domain.Adapters;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;

namespace Geekhub.EventHandler.Infrastructure.EmailAdapter.Microsoft.Extensions.DependencyInjection;

public static class EmailAdapterExtensions
{
    public static IServiceCollection AddEmailAdapter(this IServiceCollection services, Action<EmailAdapterOptions> configureOptions)
    {
        var options = new EmailAdapterOptions();
        configureOptions(options);

        if (string.IsNullOrWhiteSpace(options.Host)
            || options.Port is <= 0 or > 65535
            || !MailboxAddress.TryParse(options.FromAddress, out _))
        {
            throw new ArgumentException("Configure EmailAdapterOptions com Host, Port (1-65535) e FromAddress validos.", nameof(configureOptions));
        }

        services.AddSingleton(options);
        services.AddScoped<IEmailAdapter, EmailAdapter>();
        return services;
    }
}
