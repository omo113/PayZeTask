using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PayZe.Orders.Infrastructure.Persistence;
using PayZe.Orders.Infrastructure.Repositories.Repositories;
using PayZe.Orders.Infrastructure.Repositories.Repositories.Abstractions;
using PayZe.Orders.Infrastructure.UnitOfWork.Abstractions;
using PayZe.Shared.Abstractions;
using PayZe.Shared.Settings;

namespace PayZe.Orders.Infrastructure;

public static class DIExtension
{
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<OrdersDbContext>((sp, options) =>
        {
            var environmentSettings = sp.GetRequiredService<IOptions<EnvironmentSettings>>();
            options.UseNpgsql(environmentSettings.Value.DatabaseConnection);
        });

        services.AddHostedService<MigrationService>();
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        services.AddScoped<IMigrationDbContext, OrdersDbContext>();

        return services;
    }
}