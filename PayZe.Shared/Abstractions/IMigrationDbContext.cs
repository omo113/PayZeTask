using Microsoft.EntityFrameworkCore.Infrastructure;

namespace PayZe.Shared.Abstractions;

public interface IMigrationDbContext
{
    /// <summary>
    ///     Provides access to database related information and operations for this context.
    /// </summary>
    public DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
}