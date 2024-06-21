namespace PayZe.Orders.Application.Dtos.CompanyDtos;

public record CompanyGrpcResponse(string? ErrorMessage, CompanyGrpcResponseModel? ResponseModel);

public record CompanyGrpcResponseModel(Guid Id, string SecretHash, string ApiKey, string Salt);