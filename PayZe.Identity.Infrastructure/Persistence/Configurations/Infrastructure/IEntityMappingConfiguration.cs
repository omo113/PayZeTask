using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PayZe.Identity.Infrastructure.Persistence.Configurations.Infrastructure
{
    public interface IEntityMappingConfiguration<T> : IEntityConfiguration
        where T : class
    {
        void Map(EntityTypeBuilder<T> builder);
    }
}