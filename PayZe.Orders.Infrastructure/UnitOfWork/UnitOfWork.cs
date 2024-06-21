using MassTransit;
using PayZe.Orders.Infrastructure.Persistence;
using PayZe.Orders.Infrastructure.UnitOfWork.Abstractions;
using PayZe.Shared.Abstractions;

namespace PayZe.Orders.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly OrdersDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public UnitOfWork(OrdersDbContext dbContext, IPublishEndpoint publishEndpoint)
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