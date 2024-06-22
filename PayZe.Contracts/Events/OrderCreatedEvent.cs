using PayZe.Shared.Abstractions;

namespace PayZe.Contracts.Events;

public class OrderCreatedEvent : DomainEvent
{
    public required Guid CompanyId { get; set; }
    public required decimal Amount { get; set; }
    public required string Currency { get; set; }
}