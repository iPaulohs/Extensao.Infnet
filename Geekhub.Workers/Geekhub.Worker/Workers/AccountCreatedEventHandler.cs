namespace Geekhub.Worker.Workers;

public record AccountCreatedEvent(
    Guid Id,
    string Email,
    string FirstName,
    string? LastName
);

public class AccountCreatedEventHandler() : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}
