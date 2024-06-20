using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PayZe.Identity.Infrastructure.Persistence;
using PayZe.Identity.Infrastructure.Repositories.Repositories;
using PayZe.Identity.Infrastructure.Repositories.Repositories.Abstractions;
using PayZe.Identity.Infrastructure.UnitOfWork.Abstractions;
using PayZe.Shared.Abstractions;

namespace PayZe.Identity.Infrastructure;

public static class DIExtension
{
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetSection("//todo").Value;

        services.AddDbContext<CompanyIdentityDbContext>((sp, options) =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IMigrationDbContext, CompanyIdentityDbContext>();
        services.AddHostedService<MigrationService>();
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        services.AddScoped<IMigrationDbContext, CompanyIdentityDbContext>();

        return services;
    }
}