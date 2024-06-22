using PayZe.Shared.Abstractions;

namespace PayZe.Payment.Domain.Aggregates.OrderAggregate;

public class Order : Entity
{
    public string Currency { get; set; }
    public decimal Amount { get; set; }


    private Order(Guid id, string currency, decimal amount)
    {
        Currency = currency;
        Amount = amount;
        UId = id;
    }

    public static Order CreateOrder(Guid id, string currency, decimal amount)
    {
        return new Order(id, currency, amount);
    }
}