using MassTransit;
using Microsoft.EntityFrameworkCore;
using PayZe.Contracts.Events;
using PayZe.Payment.Application.Services;
using PayZe.Payment.Domain.Aggregates.OrderAggregate;
using PayZe.Payment.Infrastructure.Repositories.Repositories.Abstractions;

namespace PayZe.Payment.Application.Consumers;

public class PaymentCreatedConsumer : IConsumer<PaymentCreatedEvent>
{
    private readonly IRepository<Order> _orderRepository;
    private readonly PaymentToProcessChannel _channel;

    public PaymentCreatedConsumer(PaymentToProcessChannel channel, IRepository<Order> orderRepository)
    {
        _channel = channel;
        _orderRepository = orderRepository;
    }

    public async Task Consume(ConsumeContext<PaymentCreatedEvent> context)
    {
        var order = await _orderRepository.Query(x => x.UId == context.Message.OrderId).FirstAsync();
        await _channel.EnqueueJobAsync(
            new PaymentToProcess(order.Currency, order.Amount, context.Message.CardNumber, context.Message.ExpiryDate, context.Message.UId, context.Message.ProcessingService));
    }
}