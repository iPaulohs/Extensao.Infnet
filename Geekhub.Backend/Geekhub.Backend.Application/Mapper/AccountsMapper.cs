using Geekhub.Backend.Domain.Models;
using Geekhub.Backend.Domain.Results;
using Geekhub.Backend.WebApi.Dto.Request.Commands;
using Mapster;

namespace Geekhub.Backend.Application.Mapper;

public class AccountsMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateAccountCommand, User>()
            .Map(dest => dest.Email, src => new Email(src.Email))
            .Map(dest => dest.Password, src => new Password(Password.HashPassword(src.Password)));

        config.NewConfig<User, AccountDataResult>()
            .Map(dest => dest.Email, src => src.Email.Value);
    }
}
