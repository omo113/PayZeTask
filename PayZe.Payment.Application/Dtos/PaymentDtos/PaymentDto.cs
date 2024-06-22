namespace PayZe.Payment.Application.Dtos.PaymentDtos;

public record PaymentDto(DateOnly ExpiryDate, string CardNumber, Guid OrderId);