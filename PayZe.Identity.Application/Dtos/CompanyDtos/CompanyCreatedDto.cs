namespace PayZe.Identity.Application.Dtos.CompanyDtos;

public record CompanyCreatedDto(Guid Id, string CompanyName, string City, string Email, string ApiKey, string Secret, DateTimeOffset CreateDate);