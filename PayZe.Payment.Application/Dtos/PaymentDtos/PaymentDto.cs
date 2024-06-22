namespace PayZe.Payment.Application.Dtos.PaymentDtos;

public record PaymentDto(DateTimeOffset ExpiryDate, string CardNumber);