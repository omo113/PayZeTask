using Microsoft.EntityFrameworkCore;

namespace PayZe.Orders.Infrastructure.Persistence.Configurations.Infrastructure
{
    public interface IEntityConfiguration
    {
        void Map(ModelBuilder builder);
    }
}