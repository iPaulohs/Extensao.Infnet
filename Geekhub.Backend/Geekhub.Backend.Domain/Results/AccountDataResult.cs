namespace Geekhub.Backend.Domain.Results;

public record AccountDataResult(
    Guid Id,
    string Name,
    string? Surname,
    string Username,
    string DisplayName,
    string? Bio,
    string Email,
    DateTime BirthDate,
    string Token
    );
