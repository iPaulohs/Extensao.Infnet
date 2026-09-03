using Geekhub.Backend.WebApi.Routes;
using Scalar.AspNetCore;

namespace Geekhub.Backend.WebApi.Extensions
{
    public static class BuilderExtensions
    {
        public static void AddBuilderExtensions(this WebApplication app)
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
            app.MapAccountsController();
            app.MapMediaController();
        }
    }
}
