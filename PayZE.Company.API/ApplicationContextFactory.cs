using Microsoft.EntityFrameworkCore;
using PayZe.Identity.Domain.Aggregates;
using PayZe.Identity.Infrastructure.Repositories.Repositories.Abstractions;
using PayZe.Shared;

namespace PayZe.Identity.Api;

public abstract class ApplicationContextFactory
{
    public static ApplicationContext Create(IServiceProvider sp)
    {
        var httpContext = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;
        var companyRepository = sp.GetRequiredService<IRepository<Company>>();
        return Create(httpContext, companyRepository);
    }

    private static ApplicationContext Create(HttpContext? httpContext, IRepository<Company> companyRepo)
    {
        if (httpContext == null || !httpContext.Request.Headers.TryGetValue("API-Key", out var extractedApiKey) ||
            !httpContext.Request.Headers.TryGetValue("API-Secret", out var extractedApiSecret))
        {
            return new ApplicationContext(null);
        }
        var apiKey = extractedApiKey.ToString();
        var apiSecret = extractedApiSecret.ToString();
        var company = companyRepo.Query(x => x.ApiKey == apiKey).FirstOrDefaultAsync().GetAwaiter().GetResult();
        if (company == null)
        {
            return new ApplicationContext(null);
        }

        if (SecurityService.SecretEqualsHash(apiSecret, company.HashedSecret, company.Salt))
        {
            return new ApplicationContext(company.UId);
        }

        return new ApplicationContext(null);
    }
}