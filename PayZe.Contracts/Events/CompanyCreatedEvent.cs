using PayZe.Shared.Abstractions;

namespace PayZe.Contracts.Events;

public class CompanyCreatedEvent : DomainEvent
{
    public required string CompanyName { get; set; }
    public required DateTimeOffset CreateDate { get; set; }
}