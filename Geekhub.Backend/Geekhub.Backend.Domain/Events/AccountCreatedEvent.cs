namespace Geekhub.Backend.Domain.Events;

public class AccountCreatedEvent
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
}
