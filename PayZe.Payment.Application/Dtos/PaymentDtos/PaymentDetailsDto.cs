using PayZe.Shared.Enums;

namespace PayZe.Payment.Application.Dtos.PaymentDtos;

public record PaymentDetailsDto(Guid Id, DateOnly ExpiryDate, string CardNumber, Guid OrderId, OrderStatus OrderStatus, string? PaymentErrorMessage);