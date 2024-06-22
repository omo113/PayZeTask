using PayZe.Shared.Enums;

namespace PayZe.Payment.Application.Dtos.PaymentDtos;

public record PaymentDetailsDto(Guid Id, DateTimeOffset ExpiryDate, string CardNumber, Guid OrderId, OrderStatus OrderStatus, string? PaymentErrorMessage);