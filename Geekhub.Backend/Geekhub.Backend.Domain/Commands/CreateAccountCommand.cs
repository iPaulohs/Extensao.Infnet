using FluentValidation;

namespace Geekhub.Backend.WebApi.Dto.Request.Commands
{
    public record CreateAccountCommand(
        string Name,
        string? Surname,
        string Username,
        string DisplayName,
        string? Bio,
        string Email,
        string Password,
        DateOnly BirthDate
        );

    public class CreateAccountRequestValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("O nome é obrigatório.")
                .MaximumLength(20)
                .WithMessage("O nome deve ter no máximo 20 caracteres.");

            RuleFor(x => x.Surname)
                .MaximumLength(20)
                .WithMessage("O sobrenome deve ter no máximo 20 caracteres.");

            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("O nome de usuário é obrigatório.")
                .MaximumLength(30)
                .WithMessage("O nome de usuário deve ter no máximo 30 caracteres.");

            RuleFor(x => x.DisplayName)
                .NotEmpty()
                .WithMessage("O nome de exibição é obrigatório.")
                .MaximumLength(15)
                .WithMessage("O nome de exibição deve ter no máximo 15 caracteres.");

            RuleFor(x => x.Bio)
                .MaximumLength(280)
                .WithMessage("A biografia deve ter no máximo 280 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("O email é obrigatório.")
                .EmailAddress()
                .WithMessage("O email é inválido.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("A senha é obrigatória.")
                .MinimumLength(6)
                .WithMessage("A senha deve ter pelo menos 6 caracteres.");

            RuleFor(x => x.BirthDate)
                .NotEqual(DateOnly.MinValue)
                .WithMessage("A data de nascimento é obrigatória.");
        }
    }
}
