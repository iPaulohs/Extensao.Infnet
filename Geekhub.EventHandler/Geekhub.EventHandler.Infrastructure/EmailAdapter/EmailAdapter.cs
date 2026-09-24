using Geekhub.EventHandler.Domain.Adapters;
using Geekhub.EventHandler.Domain.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Geekhub.EventHandler.Infrastructure.EmailAdapter.Microsoft.Extensions.DependencyInjection;
using MimeKit;

namespace Geekhub.EventHandler.Infrastructure.EmailAdapter;

public class EmailAdapter(EmailAdapterOptions options) : IEmailAdapter
{
    public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);
        var settings = options;

        using var email = new MimeMessage();
        email.From.Add(new MailboxAddress(settings.FromName, settings.FromAddress));
        email.To.Add(MailboxAddress.Parse(message.To));
        email.Subject = message.Subject;
        email.Body = new TextPart(message.IsHtml ? "html" : "plain") { Text = message.Body };

        // Um cliente por envio permite chamadas concorrentes dos handlers.
        using var client = new SmtpClient();
        await client.ConnectAsync(settings.Host, settings.Port, SecureSocketOptions.None, cancellationToken);
        await client.SendAsync(email, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }
}
