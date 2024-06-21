namespace PayZe.Identity.Application.Dtos.CompanyDtos;

public record CompanyDetailsDto(Guid Id, string CompanyName, string City, string Email, DateTimeOffset CreateDate);