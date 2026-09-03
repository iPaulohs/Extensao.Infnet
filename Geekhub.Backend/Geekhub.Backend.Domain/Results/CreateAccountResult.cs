namespace Geekhub.Backend.Domain.Results;

public record CreateAccountResult(
    Guid Id,
    string Name,
    string? Surname,
    string Username,
    string DisplayName,
    string? Bio,
    string Email,
    DateTime BirthDate
    );
