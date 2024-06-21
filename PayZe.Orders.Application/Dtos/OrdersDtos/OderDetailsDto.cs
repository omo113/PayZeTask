namespace PayZe.Orders.Application.Dtos.OrdersDtos;

public record OderDetailsDto(Guid Id, Guid CompanyId, string Currency, decimal Amount, DateTimeOffset CreateDate);