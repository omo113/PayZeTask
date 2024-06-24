using MassTransit;
using Microsoft.EntityFrameworkCore;
using PayZe.Orders.Domain.Aggregates.CompanyAggregate;
using PayZe.Orders.Infrastructure.Persistence.Configurations;
using PayZe.Orders.Infrastructure.Persistence.Configurations.Infrastructure;
using PayZe.Shared.Abstractions;

namespace PayZe.Orders.Infrastructure.Persistence;

public class OrdersDbContext : DbContext, IMigrationDbContext
{
    public OrdersDbContext()
    {
    }
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.AddInboxStateEntity();
        builder.AddOutboxMessageEntity();
        builder.AddOutboxStateEntity();

        builder.AddConfiguration(new OrdersConfiguration());
        builder.AddConfiguration(new CompanyConfiguration());

    }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Domain.Aggregates.OrderAggregate.Order> Orders { get; set; }
}
