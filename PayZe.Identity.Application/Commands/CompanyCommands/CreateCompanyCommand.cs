using FluentValidation;
using PayZe.Identity.Application.Dtos.CompanyDtos;
using PayZe.Identity.Application.Services;
using PayZe.Identity.Domain.Aggregates;
using PayZe.Identity.Infrastructure.Repositories.Repositories.Abstractions;
using PayZe.Identity.Infrastructure.UnitOfWork.Abstractions;
using PayZe.Shared.ApplicationInfrastructure;

namespace PayZe.Identity.Application.Commands.CompanyCommands;

public record CreateCompanyCommand(CompanyDto Model) : IRequest<ApplicationResult<CompanyCreatedDto, ApplicationError>>;


public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
    {
        RuleFor(x => x.Model.CompanyName)
            .NotNull()
            .NotEmpty();
        RuleFor(x => x.Model.City)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.Model.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress();
    }
}

public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, ApplicationResult<CompanyCreatedDto, ApplicationError>>
{
    private readonly IRepository<Company> _repository;
    private readonly SecurityService _securityService;
    private readonly IUnitOfWork _unitOfWork;
    public CreateCompanyCommandHandler(IRepository<Company> repository, SecurityService securityService, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _securityService = securityService;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<CompanyCreatedDto, ApplicationError>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var apiKey = _securityService.GenerateApiKey(request.Model.CompanyName);
        var secret = _securityService.GenerateApiSecret();

        var (hash, salt) = _securityService.GenerateHash(secret);
        var model = request.Model;
        var company = Company.CreateCompany(model.CompanyName, model.City, model.Email, apiKey, hash, salt);
        await _repository.Store(company);
        await _unitOfWork.SaveAsync();
        return new ApplicationResult<CompanyCreatedDto, ApplicationError>(
            new CompanyCreatedDto(company.Id, company.Name,
                company.City, company.Email, company.ApiKey,
                company.HashedSecret, company.CreateDate));
    }
}