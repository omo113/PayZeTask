using PayZe.Shared.Abstractions;
using PayZe.Shared.Enums;

namespace PayZe.Contracts.Events;

public class PaymentCreatedEvent : DomainEvent
{
    public required Guid OrderId { get; set; }
    public required string CardNumber { get; set; }
    public required DateOnly ExpiryDate { get; set; }
    public required ProcessingService ProcessingService { get; set; }
}