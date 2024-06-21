using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using PayZe.Identity.Application.Protos;
using PayZe.Identity.Domain.Aggregates.CompanyAggregate;
using PayZe.Identity.Infrastructure.Repositories.Repositories.Abstractions;

namespace PayZe.Identity.Application.Services;

public class GrpcIdentityService : GrpcCompany.GrpcCompanyBase
{
    private readonly IRepository<Company> _repository;

    public GrpcIdentityService(IRepository<Company> repository)
    {
        _repository = repository;
    }

    public override async Task<GrpcCompanyIdentityResponse> GetAuction(GetCompanyIndetityRequest request, ServerCallContext context)
    {
        var company = await _repository.Query(x => x.ApiKey == request.ApiKey).FirstOrDefaultAsync();
        if (company is null)
        {
            return new GrpcCompanyIdentityResponse
            {
                ErrorMessage = "company with this kind of api key does not exist",
            };
        }
        return new GrpcCompanyIdentityResponse
        {
            Identity = new GrpcCompanyIdentityModel
            {
                ApiKey = company.ApiKey,
                Id = company.UId.ToString(),
                Salt = company.Salt,
                HashSecret = company.HashedSecret
            }
        };
    }
}