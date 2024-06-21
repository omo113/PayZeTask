using Microsoft.AspNetCore.Authorization;
using PayZe.Shared;

namespace PayZe.Identity.Api.Middlewares;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;

    public AuthorizationMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        _next = next;
        _serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();

        if (endpoint != null)
        {
            var allowAnonymous = endpoint.Metadata.OfType<AllowAnonymousAttribute>().Any();
            if (!allowAnonymous)
            {
                using var scope = _serviceProvider.CreateScope();
                var applicationContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                if (applicationContext.RequestingCompanyUId == null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

            }
        }
        await _next(context);
    }
}