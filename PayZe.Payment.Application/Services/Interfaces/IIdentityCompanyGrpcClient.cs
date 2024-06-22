using CompanyGrpcResponse = PayZe.Payment.Application.Dtos.CompanyDtos.CompanyGrpcResponse;

namespace PayZe.Payment.Application.Services.Interfaces;

public interface IIdentityCompanyGrpcClient
{
    Task<CompanyGrpcResponse> GetAuction(string apiKey);
}