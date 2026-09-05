using ErrorOr;
using Geekhub.Backend.Domain.Errors;
using Geekhub.Backend.Domain.Events;
using Geekhub.Backend.Domain.Models;
using Geekhub.Backend.Domain.Results;
using Geekhub.Backend.Domain.Services;
using Geekhub.Backend.Infrastructure.DatabaseAdapter;
using Geekhub.Backend.WebApi.Dto.Request.Commands;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Wolverine;

namespace Geekhub.Backend.Application.Handlers;

public static class CreateAccountHandler
{
    public static async Task<ErrorOr<AccountDataResult>> Handle(
        CreateAccountCommand command,
        IMapper mapper,
        AppDbContext dbContext,
        IMessageBus messageBus,
        IJwtHandlerService jwtHandlerService,
        CancellationToken cancellationToken)
    {
        try
        {
            var conflictCode = await dbContext.Database
            .SqlQuery<int>($@"
                SELECT 
                    CASE 
                        WHEN ""Email"" = {command.Email} THEN 1
                        WHEN ""Username"" = {command.Username} THEN 3
                        WHEN ""Email"" = {command.Email} AND ""Username"" = {command.Username} THEN 5
                        ELSE 0
                    END AS ""Value""
                FROM ""Users""
                WHERE ""Email"" = {command.Email} OR ""Username"" = {command.Username}
                LIMIT 1")
            .FirstOrDefaultAsync(cancellationToken);

            switch (conflictCode)
            {
                case 1:
                    return CreateAccountErrors.EmailAlreadyExists;
                case 3:
                    return CreateAccountErrors.UsernameAlreadyExists;
                case 5:
                    return CreateAccountErrors.EmailAndUsernameAlreadyExists;
            }

            var user = mapper.Map<User>(command);

            dbContext.Users.Add(user);

            await dbContext.SaveChangesAsync(cancellationToken);

            await messageBus.PublishAsync(new AccountCreatedEvent(
                user.Id,
                user.Email.ToString(),
                user.Name,
                user.Surname
            ));

            return mapper.Map<AccountDataResult>(user) with
            {
                Token = jwtHandlerService.GenerateToken(user)
            };
        }
        catch (Exception)
        {
            return Error.Failure();
        }
    }
}
