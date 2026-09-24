using Geekhub.EventHandler.Domain.Events;
using Wolverine.Util;

namespace Geekhub.EventHandler.Messaging;

public class IncomingEventNaming : IMessageTypeNaming
{
    public bool TryDetermineName(Type messageType, out string messageTypeName)
    {
        // Identificador recebido no Kafka; nao representa uma referencia ao assembly do produtor.
        if (messageType == typeof(AccountCreatedEvent))
        {
            messageTypeName = "Geekhub.Backend.Domain.Events.AccountCreatedEvent";
            return true;
        }

        messageTypeName = string.Empty;
        return false;
    }
}
