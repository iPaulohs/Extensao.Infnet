using Geekhub.EventHandler.Domain.Events;

namespace Geekhub.EventHandler.Handlers;

public class AccountCreatedEventHandler(ILogger<AccountCreatedEventHandler> logger)
{
    public void Handle(AccountCreatedEvent message)
    {
        logger.LogInformation("Evento AccountCreated recebido para a conta {AccountId}", message.Id);
    }
}
