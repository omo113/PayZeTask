using MassTransit;
using Microsoft.EntityFrameworkCore;
using PayZe.Identity.Domain.Aggregates;
using PayZe.Shared.Abstractions;

namespace PayZe.Identity.Infrastructure.Persistence;

public class CompanyIdentityDbContext : DbContext, IMigrationDbContext
{
    public CompanyIdentityDbContext(DbContextOptions<CompanyIdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.AddInboxStateEntity();
        builder.AddOutboxMessageEntity();
        builder.AddOutboxStateEntity();
    }
    public DbSet<Company> Companies { get; set; }
}