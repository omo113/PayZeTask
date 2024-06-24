using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PayZe.Identity.Application.Protos;
using PayZe.Payment.Application.Services.Interfaces;
using PayZe.Shared.Settings;
using CompanyGrpcResponse = PayZe.Payment.Application.Dtos.CompanyDtos.CompanyGrpcResponse;
using CompanyGrpcResponseModel = PayZe.Payment.Application.Dtos.CompanyDtos.CompanyGrpcResponseModel;

namespace PayZe.Payment.Application.Services;

public class IdentityCompanyGrpcClient : IIdentityCompanyGrpcClient
{
    private readonly ILogger<IdentityCompanyGrpcClient> _logger;
    private readonly IdentitySettings _settings;

    public IdentityCompanyGrpcClient(ILogger<IdentityCompanyGrpcClient> logger, IOptions<IdentitySettings> settings)
    {
        _logger = logger;
        _settings = settings.Value;
    }
    public async Task<CompanyGrpcResponse> GetAuction(string apiKey)
    {
        _logger.LogInformation("Calling GRPC Service");
        var client = new GrpcCompany.GrpcCompanyClient(GrpcChannel.ForAddress(_settings.IdentityUrl));
        var request = new GetCompanyIndetityRequest
        {
            ApiKey = apiKey
        };
        try
        {
            var reply = await client.GetAuctionAsync(request);
            if (reply.Identity is not null && !string.IsNullOrEmpty(reply.Identity.Id))
            {
                return new CompanyGrpcResponse(null,
                    new CompanyGrpcResponseModel(Guid.Parse(reply.Identity.Id),
                        reply.Identity.HashedSecret,
                        reply.Identity.ApiKey,
                        reply.Identity.Salt));
            }
            return new CompanyGrpcResponse(reply.ErrorMessage, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the company identity.");
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }
}