using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PayZe.Orders.Application.Services.Interfaces;
using PayZe.Shared;

namespace PayZe.Orders.Api.Middlewares
{
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
                    var secret = extractedApiSecret.ToString();

                    using var scope = _serviceProvider.CreateScope();
                    var identityCompanyGrpcClient = scope.ServiceProvider.GetRequiredService<IIdentityCompanyGrpcClient>();
                    var res = await identityCompanyGrpcClient.GetAuction(apiKey);

                    if (res.ErrorMessage is not null && res.ErrorMessage != "")
                    {
                        context.Result = new UnauthorizedObjectResult(res.ErrorMessage);
                        return;
                    }

                    if (!SecurityService.SecretEqualsHash(secret, res.ResponseModel!.SecretHash, res.ResponseModel.Salt))
                    {
                        context.Result = new UnauthorizedObjectResult("Unauthorized: Invalid API Key or Secret.");
                        return;
                    }

                    // Set the CompanyId header
                    context.HttpContext.Request.Headers["CompanyId"] = res.ResponseModel.Id.ToString();
                }
            }

            await next();
        }
    }
}
