using FluentValidation;

namespace Geekhub.Backend.WebApi.Filters
{
    public class ValidationFilter<T> : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
            var argument = context.Arguments.OfType<T>().FirstOrDefault();

            if (validator is null || argument is null)
            {
                return await next(context);
            }

            var result = await validator.ValidateAsync(argument);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.ToDictionary());
            }

            return await next(context);
        }
    }
}
