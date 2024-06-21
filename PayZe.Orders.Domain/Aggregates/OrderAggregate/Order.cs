using PayZe.Orders.Domain.Aggregates.CompanyAggregate;
using PayZe.Shared.Abstractions;

namespace PayZe.Orders.Domain.Aggregates.OrderAggregate;

public class Order : AggregateRoot
{
    public int CompanyId { get; private set; }
    public Company Company { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }

    private Order()
    {

    }
    private Order(Company company, decimal amount, string currency)
    {
        Company = company;
        Amount = amount;
        Currency = currency;
    }

    public static Order CreateOrder(Company company, decimal amount, string currency)
    {
        return new Order(company, amount, currency);
    }
}