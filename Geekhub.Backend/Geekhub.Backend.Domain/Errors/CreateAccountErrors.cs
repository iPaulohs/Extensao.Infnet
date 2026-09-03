using ErrorOr;

namespace Geekhub.Backend.Domain.Errors;

public static class CreateAccountErrors
{
    public static Error EmailAlreadyExists => Error.Validation(
        code: "EmailAlreadyExists",
        description: "O e-mail informado já está em uso.");

    public static Error UsernameAlreadyExists => Error.Validation(
        code: "UsernameAlreadyExists",
        description: "O nome de usuário informado já está em uso.");

    public static Error EmailAndUsernameAlreadyExists => Error.Validation(
        code: "EmailAndUsernameAlreadyExists",
        description: "O e-mail e o nome de usuário informados já estão em uso.");
}
