namespace Geekhub.EventHandler.Infrastructure.EmailAdapter.Microsoft.Extensions.DependencyInjection;

public class EmailAdapterOptions
{
    public const string SectionName = "EmailAdapterOptions";

    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 1025;
    public string FromAddress { get; set; } = "noreply@geekhub.local";
    public string FromName { get; set; } = "Geekhub";
}
