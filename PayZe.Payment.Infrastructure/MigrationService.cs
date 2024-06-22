using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PayZe.Payment.Infrastructure.Persistence;
using PayZe.Shared.Abstractions;

namespace PayZe.Payment.Infrastructure;

public class MigrationService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<MigrationService> _logger;
    public MigrationService(ILogger<MigrationService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;

    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return ExecuteAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await RunMigration(stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "");
        }
    }

    private async Task RunMigration(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var migrationContext = scope.ServiceProvider.GetRequiredService<IMigrationDbContext>();
        await RunContextMigration(scope.ServiceProvider.GetRequiredService<PaymentDbContext>().Database, cancellationToken);
        await RunContextMigration(migrationContext.Database, cancellationToken);
    }

    private async Task RunContextMigration(DatabaseFacade database, CancellationToken cancellationToken)
    {
        var pendingMigrations = await database.GetPendingMigrationsAsync(cancellationToken);
        if (pendingMigrations.Any())
        {
            await database.MigrateAsync(cancellationToken);
        }
    }
}