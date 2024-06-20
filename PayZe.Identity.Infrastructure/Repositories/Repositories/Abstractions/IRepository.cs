using System.Linq.Expressions;

namespace PayZe.Identity.Infrastructure.Repositories.Repositories.Abstractions
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Query(Expression<Func<T, bool>>? expression = null, bool onlyActives = true);

        Task Store(T document);
    }
}