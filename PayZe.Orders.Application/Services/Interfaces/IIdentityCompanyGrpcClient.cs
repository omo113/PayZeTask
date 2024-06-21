using PayZe.Orders.Application.Dtos.CompanyDtos;

namespace PayZe.Orders.Application.Services.Interfaces;

public interface IIdentityCompanyGrpcClient
{
    Task<CompanyGrpcResponse> GetAuction(string apiKey);
}