using Geekhub.Backend.Domain.Models;

namespace Geekhub.Backend.Domain.Services;

public interface IJwtHandlerService
{
    string GenerateToken(User account);
}
