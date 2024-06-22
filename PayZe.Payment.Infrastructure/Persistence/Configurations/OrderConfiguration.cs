using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayZe.Payment.Domain.Aggregates.OrderAggregate;
using PayZe.Payment.Infrastructure.Persistence.Configurations.Infrastructure;


namespace PayZe.Payment.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : EntityConfiguration<Order>
{
    public override void Map(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders").HasKey(x => x.Id);
        builder.HasAlternateKey(x => x.UId);
    }
}