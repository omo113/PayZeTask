using MassTransit;
using PayZe.Identity.Infrastructure.Persistence;
using PayZe.Identity.Infrastructure.UnitOfWork.Abstractions;
using PayZe.Shared.Abstractions;

namespace PayZe.Identity.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly CompanyIdentityDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public UnitOfWork(CompanyIdentityDbContext dbContext, IPublishEndpoint publishEndpoint)
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
        foreach (var aggregate in aggregates)
        {
            aggregate.Entity.IncreaseVersion();
        }

        var domainEventPublishTasks = new List<Task>();
        foreach (var item in events)
        {
            domainEventPublishTasks.Add(_publishEndpoint.Publish(item));
        }

        await Task.WhenAll(domainEventPublishTasks);
        return await _dbContext.SaveChangesAsync();
    }
}