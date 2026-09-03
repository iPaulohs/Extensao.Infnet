using ErrorOr;
using Geekhub.Backend.Domain.Errors;
using Geekhub.Backend.Domain.Events;
using Geekhub.Backend.Domain.Models;
using Geekhub.Backend.Domain.Results;
using Geekhub.Backend.Infrastructure.DatabaseAdapter;
using Geekhub.Backend.WebApi.Dto.Request.Commands;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Wolverine;

namespace Geekhub.Backend.Application.Handlers;

public static class CreateAccountHandler
{
    public static async Task<ErrorOr<CreateAccountResult>> Handle(
        CreateAccountCommand request,
        IMapper mapper,
        AppDbContext dbContext,
        IMessageBus messageBus,
        CancellationToken cancellationToken)
    {
        try
        {
            var conflictCode = await dbContext.Database
            .SqlQuery<int>($@"
                SELECT 
                    CASE 
                        WHEN ""Email"" = {request.Email} THEN 1
                        WHEN ""Username"" = {request.Username} THEN 3
                        WHEN ""Email"" = {request.Email} AND ""Username"" = {request.Username} THEN 5
                        ELSE 0
                    END AS ""Value""
                FROM ""Users""
                WHERE ""Email"" = {request.Email} OR ""Username"" = {request.Username}
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

            var user = mapper.Map<Account>(request);

            dbContext.Users.Add(user);

            await dbContext.SaveChangesAsync(cancellationToken);

            await messageBus.PublishAsync(new AccountCreatedEvent
            {
                Id = user.Id,
                Email = user.Email.ToString()
            });

            return mapper.Map<CreateAccountResult>(user);
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}
