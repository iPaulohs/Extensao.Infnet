using ErrorOr;

namespace Geekhub.Backend.Domain.Errors;

public static class LoginErrors
{
    public static Error InvalidCredentials => Error.Unauthorized(
        code: "Credenciais inválidas.",
        description: "O e-mail ou a senha informados estão incorretos.");
}
