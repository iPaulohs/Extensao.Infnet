using Geekhub.Backend.Domain.Models;
using Geekhub.Backend.Domain.Results;
using Geekhub.Backend.WebApi.Dto.Request.Commands;
using Mapster;

namespace Geekhub.Backend.Application.Mapper;

public class AccountsMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateAccountCommand, Account>()
            .Map(dest => dest.Email, src => new Email(src.Email))
            .Map(dest => dest.Password, src => new Password(src.Password));

        config.NewConfig<Account, CreateAccountResult>()
            .Map(dest => dest.Email, src => src.Email.Value);
    }
}
