using MassTransit;
using PayZe.Contracts.Events;
using PayZe.Orders.Domain.Aggregates.OrderAggregate;
using PayZe.Orders.Infrastructure.Repositories.Repositories.Abstractions;
using PayZe.Orders.Infrastructure.UnitOfWork.Abstractions;

namespace PayZe.Orders.Application.Consumers;

public class PaymentProcessedEventConsumer : IConsumer<PaymentProcessedEvent>
{
    private IRepository<Order> _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    public PaymentProcessedEventConsumer(IRepository<Order> orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public Task Consume(ConsumeContext<PaymentProcessedEvent> context)
    {
        throw new NotImplementedException();
    }
}