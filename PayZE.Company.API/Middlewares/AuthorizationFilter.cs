using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using PayZe.Identity.Domain.Aggregates.CompanyAggregate;
using PayZe.Identity.Infrastructure.Repositories.Repositories.Abstractions;
using PayZe.Shared;

namespace PayZe.Identity.Api.Middlewares;

public class AuthorizationFilter : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider;

    public AuthorizationFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var endpoint = context.HttpContext.GetEndpoint();

        if (endpoint != null)
        {
            var allowAnonymous = endpoint.Metadata.OfType<AllowAnonymousAttribute>().Any();
            if (!allowAnonymous)
            {
                if (!context.HttpContext.Request.Headers.TryGetValue("API-Key", out var extractedApiKey) ||
                    !context.HttpContext.Request.Headers.TryGetValue("API-Secret", out var extractedApiSecret))
                {
                    context.Result = new UnauthorizedObjectResult("Unauthorized: missing API-Key or API-Secret header");
                    return;
                }

                var apiKey = extractedApiKey.ToString();
                var apiSecret = extractedApiSecret.ToString();

                using var scope = _serviceProvider.CreateScope();
                var companyRepository = scope.ServiceProvider.GetRequiredService<IRepository<Company>>();
                var company = await companyRepository.Query(x => x.ApiKey == apiKey).FirstOrDefaultAsync();

                if (company == null)
                {
                    context.Result = new UnauthorizedObjectResult("Unauthorized: Invalid API Key.");
                    return;
                }

                if (!SecurityService.SecretEqualsHash(apiSecret, company.HashedSecret, company.Salt))
                {
                    context.Result = new UnauthorizedObjectResult("Unauthorized: Invalid API Secret.");
                    return;
                }

                // Set the CompanyId header
                context.HttpContext.Request.Headers["CompanyId"] = company.UId.ToString();
            }
        }

        await next();
    }
}