using PayZe.Shared;
using PayZe.Shared.Abstractions;

namespace PayZe.Orders.Domain.Aggregates.CompanyAggregate;

public class Company : Entity
{
    public string Name { get; private set; }
    private readonly List<OrderAggregate.Order> _orders = new();
    public IEnumerable<OrderAggregate.Order> Orders => _orders.Where(x => x.Active());

    private Company()
    {

    }
    private Company(Guid id, string name)
    {
        UId = id;
        Name = name;
        CreateDate = SystemDate.Now;
    }


    public static Company CreateCompany(Guid id, string name)
    {
        return new Company(id, name);
    }
}