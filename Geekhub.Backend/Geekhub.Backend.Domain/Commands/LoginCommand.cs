using FluentValidation;

namespace Geekhub.Backend.Domain.Commands;

public record LoginCommand(string UserIdentifier, string Password);

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.UserIdentifier)
                .NotEmpty()
                .WithMessage("O identificador do usuário é obrigatório.");


        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("A senha é obrigatória.");
    }
}

