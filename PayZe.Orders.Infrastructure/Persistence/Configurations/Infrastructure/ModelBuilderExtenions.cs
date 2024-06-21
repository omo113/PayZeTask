using Microsoft.EntityFrameworkCore;

namespace PayZe.Orders.Infrastructure.Persistence.Configurations.Infrastructure
{
    public static class ModelBuilderExtenions
    {
        public static void AddConfiguration<TEntity>(this ModelBuilder modelBuilder, EntityConfiguration<TEntity> configuration)
            where TEntity : class
        {
            configuration.Map(modelBuilder);
        }
    }
}