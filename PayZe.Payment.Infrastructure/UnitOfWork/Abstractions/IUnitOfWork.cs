namespace PayZe.Payment.Infrastructure.UnitOfWork.Abstractions;

public interface IUnitOfWork
{
    Task<int> SaveAsync();
}