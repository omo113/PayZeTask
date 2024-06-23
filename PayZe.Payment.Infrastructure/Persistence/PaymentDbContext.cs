using MassTransit;
using Microsoft.EntityFrameworkCore;
using PayZe.Payment.Infrastructure.Persistence.Configurations;
using PayZe.Payment.Infrastructure.Persistence.Configurations.Infrastructure;
using PayZe.Shared.Abstractions;

namespace PayZe.Payment.Infrastructure.Persistence;

public class PaymentDbContext : DbContext, IMigrationDbContext
{
    public PaymentDbContext()
    {
    }
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
        : base(options)
    {
    }
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseNpgsql("Host=localhost; Database=dotnet-PayZe-Payment; Username=myuser; Password=mypassword;Pooling=true;");
    //}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.AddInboxStateEntity();
        builder.AddOutboxMessageEntity();
        builder.AddOutboxStateEntity();

        builder.AddConfiguration(new PaymentConfiguration());
        builder.AddConfiguration(new OrderConfiguration());
    }
}
