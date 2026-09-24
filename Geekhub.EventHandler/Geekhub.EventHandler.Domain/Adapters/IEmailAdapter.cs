using Geekhub.EventHandler.Domain.Models;

namespace Geekhub.EventHandler.Domain.Adapters;

public interface IEmailAdapter
{
    Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default);
}
