using Microsoft.EntityFrameworkCore;

namespace PayZe.Payment.Infrastructure.Persistence.Configurations.Infrastructure
{
    public interface IEntityConfiguration
    {
        void Map(ModelBuilder builder);
    }
}