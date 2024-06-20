namespace PayZe.Identity.Infrastructure.UnitOfWork.Abstractions;

public interface IUnitOfWork
{
    Task<int> SaveAsync();
}