using MassTransit;
using PayZe.Contracts.Events;
using PayZe.Payment.Domain.Aggregates.OrderAggregate;
using PayZe.Payment.Infrastructure.Repositories.Repositories.Abstractions;
using PayZe.Payment.Infrastructure.UnitOfWork.Abstractions;

namespace PayZe.Payment.Application.Consumers;

public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly IRepository<Order> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderCreatedConsumer(IRepository<Order> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var order = Order.CreateOrder(context.Message.UId, context.Message.Currency, context.Message.Amount);
        await _repository.Store(order);
        await _unitOfWork.SaveAsync();
    }
}