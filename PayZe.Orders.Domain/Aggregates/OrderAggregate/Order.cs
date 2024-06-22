using PayZe.Contracts.Events;
using PayZe.Orders.Domain.Aggregates.CompanyAggregate;
using PayZe.Shared;
using PayZe.Shared.Abstractions;
using PayZe.Shared.Enums;

namespace PayZe.Orders.Domain.Aggregates.OrderAggregate;

public sealed class Order : AggregateRoot
{
    public int CompanyId { get; private set; }
    public Company Company { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public string? PaymentErrorMessage { get; private set; }
    private Order()
    {

    }
    private Order(Company company, decimal amount, string currency)
    {
        UId = Guid.NewGuid();
        Company = company;
        Amount = amount;
        Currency = currency;
        OrderStatus = OrderStatus.Processing;
        Raise(new OrderCreatedEvent
        {
            Amount = amount,
            Currency = currency,
            CompanyId = Company.UId
        });
    }

    public static Order CreateOrder(Company company, decimal amount, string currency)
    {
        return new Order(company, amount, currency);
    }

    public void UpdateOrderStatus(OrderStatus orderStatus, string? paymentErrorMessage)
    {
        OrderStatus = orderStatus;
        PaymentErrorMessage = paymentErrorMessage;
        LastChangeDate = SystemDate.Now;
    }
}