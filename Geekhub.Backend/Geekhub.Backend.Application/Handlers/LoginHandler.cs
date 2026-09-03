using ErrorOr;
using Geekhub.Backend.Domain.Commands;
using Geekhub.Backend.Domain.Errors;
using Geekhub.Backend.Domain.Results;
using Geekhub.Backend.Domain.Services;
using Geekhub.Backend.Infrastructure.DatabaseAdapter;
using MapsterMapper;

namespace Geekhub.Backend.Application.Handlers;

public static class LoginHandler
{
    public static ErrorOr<AccountDataResult> Handle(
        LoginCommand command,
        AppDbContext dbContext,
        IMapper mapper,
        IJwtHandlerService jwtHandler)
    {
        var account = dbContext.Users
            .Where(x => x.Email.Value == command.UserIdentifier || x.Username == command.UserIdentifier)
            .FirstOrDefault();

        if (account is null)
        {
            return LoginErrors.InvalidCredentials;
        }

        if (!account.Password.Verify(command.Password))
        {
            return LoginErrors.InvalidCredentials;
        }

        return mapper.Map<AccountDataResult>(account) with
        {
            Token = jwtHandler.GenerateToken(account)
        };
    }
}
