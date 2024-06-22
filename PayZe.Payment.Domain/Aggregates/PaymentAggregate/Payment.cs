using PayZe.Contracts.Events;
using PayZe.Payment.Domain.Aggregates.OrderAggregate;
using PayZe.Shared;
using PayZe.Shared.Abstractions;
using PayZe.Shared.Enums;

namespace PayZe.Payment.Domain.Aggregates.PaymentAggregate;

public sealed class Payment : AggregateRoot
{
    public int OrderId { get; private set; }
    public Order Order { get; private set; }
    public string CardNumber { get; private set; }
    public DateOnly ExpiryDate { get; private set; }
    public ProcessingService ProcessingService { get; private set; }
    public OrderStatus Status { get; private set; }
    public string? PaymentErrorMessage { get; private set; }
    public DateTimeOffset ResponseDate { get; private set; }

    private Payment()
    {

    }
    private Payment(Order order, string cardNumber, DateOnly expiryDate, ProcessingService processingService, OrderStatus status)
    {
        UId = Guid.NewGuid();
        CardNumber = cardNumber;
        ExpiryDate = expiryDate;
        ProcessingService = processingService;
        Status = status;
        Order = order;
        Raise(new PaymentCreatedEvent
        {
            CardNumber = CardNumber,
            ExpiryDate = ExpiryDate,
            OrderId = Order.UId,
            ProcessingService = ProcessingService,
        });
    }

    public static Payment CreatePayment(Order order, string cardNumber, DateOnly expiryDate,
        ProcessingService processingService, OrderStatus status)
    {
        return new Payment(order, cardNumber, expiryDate, processingService, status);
    }

    public void UpdatePaymentStatus(OrderStatus status, string? paymentErrorMessage, DateTimeOffset responseTime)
    {
        Status = status;
        PaymentErrorMessage = paymentErrorMessage;
        ResponseDate = responseTime;
        LastChangeDate = SystemDate.Now;
        Raise(new PaymentProcessedEvent
        {
            PaymentErrorMessage = paymentErrorMessage,
            ResponseDate = responseTime,
            Status = status,
            OrderId = Order.UId,
        });
    }
}