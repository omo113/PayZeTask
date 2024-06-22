namespace PayZe.Payment.Application.Services.Interfaces;


public record ComputeOrderDto(
    Guid PaymentId,
    decimal Amount,
    string CardNumber,
    string Currency,
    DateOnly ExpiryDate);
public interface IPaymentProcessor
{
    Task ComputeOrder(ComputeOrderDto computeOrderDto, CancellationToken cancellationToken);
}