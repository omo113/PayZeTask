using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayZe.Payment.Infrastructure.Persistence.Configurations.Infrastructure;

namespace PayZe.Payment.Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : EntityConfiguration<Domain.Aggregates.PaymentAggregate.Payment>
{
    public override void Map(EntityTypeBuilder<Domain.Aggregates.PaymentAggregate.Payment> builder)
    {
        builder.ToTable("Payment").HasKey(x => x.Id);
        builder.HasAlternateKey(x => x.UId);
        builder.Property(x => x.UId).HasDefaultValueSql("gen_random_uuid()");
        builder.HasOne(x => x.Order)
            .WithOne()
            .HasForeignKey<Domain.Aggregates.PaymentAggregate.Payment>(x => x.OrderId);
    }
}