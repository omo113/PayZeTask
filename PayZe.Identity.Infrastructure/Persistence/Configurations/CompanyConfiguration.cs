using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayZe.Identity.Domain.Aggregates;
using PayZe.Identity.Infrastructure.Persistence.Configurations.Infrastructure;

namespace PayZe.Identity.Infrastructure.Persistence.Configurations;

internal class CompanyConfiguration : EntityConfiguration<Company>
{
    public override void Map(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies").HasKey(x => x.Id);
        builder.HasAlternateKey(x => x.UId);
        builder.Property(x => x.UId).HasDefaultValueSql("gen_random_uuid()");

        builder
            .HasIndex(e => new { e.HashedSecret, e.ApiKey })
            .HasDatabaseName("API_KEY_HASH_SECRET");
    }
}