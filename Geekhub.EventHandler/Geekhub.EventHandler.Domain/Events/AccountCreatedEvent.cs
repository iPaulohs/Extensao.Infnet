namespace Geekhub.EventHandler.Domain.Events;

public record AccountCreatedEvent(
    Guid Id,
    string Email,
    string FirstName,
    string? LastName
) : IEvent;
