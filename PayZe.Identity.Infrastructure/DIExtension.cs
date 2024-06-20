using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PayZe.Identity.Infrastructure.Persistence;
using PayZe.Identity.Infrastructure.Repositories.Repositories;
using PayZe.Identity.Infrastructure.Repositories.Repositories.Abstractions;
using PayZe.Identity.Infrastructure.UnitOfWork.Abstractions;
using PayZe.Shared.Abstractions;
using PayZe.Shared.Settings;

namespace PayZe.Identity.Infrastructure;

public static class DIExtension
{
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<IdentityDbContext>((sp, options) =>
        {
            var environmentSettings = sp.GetRequiredService<IOptions<EnvironmentSettings>>();
            options.UseNpgsql(environmentSettings.Value.DatabaseConnection);
        });

        services.AddHostedService<MigrationService>();
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        services.AddScoped<IMigrationDbContext, IdentityDbContext>();

        return services;
    }
}