using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayZe.Orders.Domain.Aggregates.OrderAggregate;
using PayZe.Orders.Infrastructure.Persistence.Configurations.Infrastructure;

namespace PayZe.Orders.Infrastructure.Persistence.Configurations;

internal class OrdersConfiguration : EntityConfiguration<Order>
{
    public override void Map(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders").HasKey(x => x.Id);
        builder.HasAlternateKey(x => x.UId);
        builder.Property(x => x.UId).HasDefaultValueSql("gen_random_uuid()");

        builder.HasOne(x => x.Company)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.CompanyId);
    }
}