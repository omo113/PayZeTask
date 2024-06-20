using Microsoft.EntityFrameworkCore;

namespace PayZe.Identity.Infrastructure.Persistence.Configurations.Infrastructure
{
    public interface IEntityConfiguration
    {
        void Map(ModelBuilder builder);
    }
}