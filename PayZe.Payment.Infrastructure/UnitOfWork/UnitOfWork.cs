using MassTransit;
using PayZe.Payment.Infrastructure.Persistence;
using PayZe.Payment.Infrastructure.UnitOfWork.Abstractions;
using PayZe.Shared.Abstractions;

namespace PayZe.Payment.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly PaymentDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public UnitOfWork(PaymentDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<int> SaveAsync()
    {
        var aggregates = _dbContext.ChangeTracker.Entries<AggregateRoot>()
                                   .Select(x => x)
                                   .ToList();

        var events = _dbContext.ChangeTracker.Entries<IHasDomainEvent>()
                               .Select(x => x.Entity.PendingDomainEvents())
                               .SelectMany(x => x)
                               .ToList();

        var domainEventPublishTasks = new List<Task>();
        foreach (var item in events)
        {
            domainEventPublishTasks.Add(_publishEndpoint.Publish(item));
        }

        await Task.WhenAll(domainEventPublishTasks);
        return await _dbContext.SaveChangesAsync();
    }
}