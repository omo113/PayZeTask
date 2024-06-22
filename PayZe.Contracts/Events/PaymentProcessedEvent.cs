using PayZe.Shared.Abstractions;
using PayZe.Shared.Enums;

namespace PayZe.Contracts.Events;

public class PaymentProcessedEvent : DomainEvent
{
    public required OrderStatus Status { get; set; }
    public required string? PaymentErrorMessage { get; set; }
    public required DateTimeOffset ResponseDate { get; set; }
    public required Guid OrderId { get; set; }
}