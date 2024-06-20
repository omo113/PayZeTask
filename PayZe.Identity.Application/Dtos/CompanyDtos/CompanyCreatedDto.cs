namespace PayZe.Identity.Application.Dtos.CompanyDtos;

public record CompanyCreatedDto(int CompanyId, string CompanyName, string City, string Email, string ApiKey, string Secret, DateTimeOffset CreateDate);