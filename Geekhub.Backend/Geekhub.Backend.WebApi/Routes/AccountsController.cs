using ErrorOr;
using Geekhub.Backend.Domain.Adapters;
using Geekhub.Backend.Domain.Commands;
using Geekhub.Backend.Domain.Results;
using Geekhub.Backend.WebApi.Dto.Request.Commands;
using Geekhub.Backend.WebApi.Filters;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TMDbLib.Client;
using Wolverine;

namespace Geekhub.Backend.WebApi.Routes;

public static class AccountsController
{
    private record AccountsControllerServices(
        [FromServices] IMapper Mapper,
        [FromServices] TMDbClient TmdbClient,
        [FromServices] IRedisAdapter RedisAdapter,
        [FromServices] IMessageBus MessageBus,
        ClaimsPrincipal Claims
    );

    public static void MapAccountsController(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/accounts")
            .WithTags("Accounts")
            .WithDescription("Operações relacionadas às contas de usuário.");

        group.MapPost("/create", async Task<IResult> (CreateAccountCommand request, [AsParameters] AccountsControllerServices Services) =>
        {
            var result = await Services
                .MessageBus
                .InvokeAsync<ErrorOr<AccountDataResult>>(request);

            if (result.IsError)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.Ok(result.Value);
        })
            .WithName("CreateAccount")
            .WithSummary("Cria uma nova conta")
            .WithDescription("Realiza a criação de uma nova conta de usuário no sistema.")
            .AddEndpointFilter<ValidationFilter<CreateAccountCommand>>()
            .Produces<ErrorOr<AccountDataResult>>();


        group.MapPost("/login", async Task<IResult> (LoginCommand command, [AsParameters] AccountsControllerServices Services) =>
        {
            var result = await Services
                .MessageBus
                .InvokeAsync<ErrorOr<AccountDataResult>>(command);

            if (result.IsError)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.Ok(result.Value);
        })
            .WithName("Login")
            .WithSummary("Realiza o login")
            .WithDescription("Autentica um usuário utilizando suas credenciais e inicia uma sessão autenticada.")
            .AddEndpointFilter<ValidationFilter<LoginCommand>>()
            .Produces<ErrorOr<AccountDataResult>>()
            .AllowAnonymous();

        group.MapGet("/id", async Task<IResult> ([AsParameters] AccountsControllerServices Services) =>
        {
            throw new NotImplementedException();
        })
            .WithName("GetAccountById")
            .WithSummary("Obtém uma conta pelo ID")
            .WithDescription("Retorna os dados da conta de usuário correspondente ao identificador informado.")
            .RequireAuthorization();

        group.MapGet("/username", async Task<IResult> (string username, [AsParameters] AccountsControllerServices Services) =>
        {
            throw new NotImplementedException();
        })
            .WithName("GetAccountByUsername")
            .WithSummary("Obtém uma conta pelo nome de usuário")
            .WithDescription("Retorna os dados da conta de usuário correspondente ao nome de usuário informado.")
            .RequireAuthorization();

        group.MapGet("/email", async Task<IResult> (string email, [AsParameters] AccountsControllerServices Services) =>
        {
            throw new NotImplementedException();
        })
            .WithName("GetAccountByEmail")
            .WithSummary("Obtém uma conta pelo email")
            .WithDescription("Retorna os dados da conta de usuário associada ao endereço de email informado.")
            .RequireAuthorization();

        group.MapDelete("/", async Task<IResult> (Guid id, [AsParameters] AccountsControllerServices Services) =>
        {
            throw new NotImplementedException();
        })
            .WithName("DeleteAccount")
            .WithSummary("Exclui uma conta")
            .WithDescription("Exclui permanentemente a conta de usuário correspondente ao identificador informado.")
            .RequireAuthorization();

        group.MapPatch("/profile/pic", async Task<IResult> (Guid id, [AsParameters] AccountsControllerServices Services) =>
        {
            throw new NotImplementedException();
        })
            .WithName("UpdateProfilePicture")
            .WithSummary("Atualiza a foto de perfil")
            .WithDescription("Atualiza a foto de perfil da conta de usuário correspondente ao identificador informado.")
            .RequireAuthorization();

        group.MapGet("/profile/pic", async Task<IResult> (Guid id, [AsParameters] AccountsControllerServices Services) =>
        {
            throw new NotImplementedException();
        })
            .WithName("GetProfilePicture")
            .WithSummary("Obtém a foto de perfil")
            .WithDescription("Retorna a foto de perfil da conta de usuário correspondente ao identificador informado.")
            .RequireAuthorization();

        group.MapDelete("/profile/pic", async Task<IResult> (Guid id, [AsParameters] AccountsControllerServices Services) =>
        {
            throw new NotImplementedException();
        })
            .WithName("DeleteProfilePicture")
            .WithSummary("Remove a foto de perfil")
            .WithDescription("Remove a foto de perfil da conta de usuário correspondente ao identificador informado.")
            .RequireAuthorization();

        group.MapPatch("/profile/email", async Task<IResult> (Guid id, string newEmail, [AsParameters] AccountsControllerServices Services) =>
        {
            throw new NotImplementedException();
        })
            .WithName("UpdateAccountEmail")
            .WithSummary("Atualiza o email da conta")
            .WithDescription("Altera o endereço de email associado à conta de usuário.")
            .RequireAuthorization();

        group.MapPatch("/profile/password", async Task<IResult> (Guid id, [AsParameters] AccountsControllerServices Services) =>
        {
            throw new NotImplementedException();
        })
            .WithName("UpdateAccountPassword")
            .WithSummary("Atualiza a senha da conta")
            .WithDescription("Altera a senha utilizada para autenticação na conta de usuário.")
            .RequireAuthorization();

        group.MapPatch("/profile/username", async Task<IResult> (Guid id, string newUsername, [AsParameters] AccountsControllerServices Services) =>
        {
            throw new NotImplementedException();
        })
            .WithName("UpdateAccountUsername")
            .WithSummary("Atualiza o nome de usuário")
            .WithDescription("Altera o nome de usuário associado à conta.")
            .RequireAuthorization();

        group.MapPost("/token/refresh", async Task<IResult> ([AsParameters] AccountsControllerServices Services) =>
        {
            throw new NotImplementedException();
        })
            .WithName("RefreshToken")
            .WithSummary("Atualiza o token de acesso")
            .WithDescription("Gera um novo token de acesso utilizando um token de atualização válido.")
            .RequireAuthorization();

        group.MapPost("/profile/password/forgot", async Task<IResult> ([AsParameters] AccountsControllerServices Services) =>
        {
            throw new NotImplementedException();
        })
            .WithName("ForgotPassword")
            .WithSummary("Solicita recuperação de senha")
            .WithDescription("Inicia o processo de recuperação de senha para a conta associada às informações fornecidas.")
            .RequireAuthorization();

        group.MapPost("/profile/password/reset", async Task<IResult> ([AsParameters] AccountsControllerServices Services) =>
        {
            throw new NotImplementedException();
        })
            .WithName("ResetPassword")
            .WithSummary("Redefine a senha")
            .WithDescription("Redefine a senha da conta utilizando as informações de recuperação fornecidas.")
            .RequireAuthorization();

        group.MapPost("/profile/email/verify", async Task<IResult> ([AsParameters] AccountsControllerServices Services) =>
        {
            throw new NotImplementedException();
        })
            .WithName("VerifyEmail")
            .WithSummary("Verifica o endereço de email")
            .WithDescription("Valida o endereço de email da conta utilizando o código ou token de verificação fornecido.")
            .RequireAuthorization();
    }
}