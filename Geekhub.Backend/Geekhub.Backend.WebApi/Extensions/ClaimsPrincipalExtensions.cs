using System.Security.Claims;

namespace Geekhub.Backend.WebApi.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid? GetUserId(this ClaimsPrincipal principal)
        {
            var claimValue = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? principal.FindFirst("sub")?.Value
                          ?? principal.FindFirst("id")?.Value;

            return Guid.TryParse(claimValue, out var userId) ? userId : null;
        }
    }
}
