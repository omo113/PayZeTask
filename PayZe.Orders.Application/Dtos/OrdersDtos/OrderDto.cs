namespace PayZe.Orders.Application.Dtos.OrdersDtos;



public record OrderDto(Guid CompanyId, string Currency, decimal Amount);