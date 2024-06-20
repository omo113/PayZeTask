namespace PayZe.Identity.Application.Dtos.CompanyDtos;

public record CompanyDetailsDto(int Id, string CompanyName, string City, string Email, DateTimeOffset CreateDate);