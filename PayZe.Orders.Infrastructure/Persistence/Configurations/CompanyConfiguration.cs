using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayZe.Orders.Domain.Aggregates.CompanyAggregate;
using PayZe.Orders.Infrastructure.Persistence.Configurations.Infrastructure;

namespace PayZe.Orders.Infrastructure.Persistence.Configurations;

internal class CompanyConfiguration : EntityConfiguration<Company>
{
    public override void Map(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies").HasKey(x => x.Id);
        builder.HasAlternateKey(x => x.UId);

    }
}