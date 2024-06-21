using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using PayZe.Identity.Application.Dtos.CompanyDtos;
using PayZe.Identity.Domain.Aggregates.CompanyAggregate;
using PayZe.Identity.Infrastructure.Repositories.Repositories.Abstractions;
using PayZe.Identity.Infrastructure.UnitOfWork.Abstractions;
using PayZe.Shared;
using PayZe.Shared.ApplicationInfrastructure;

namespace PayZe.Identity.Application.Commands.CompanyCommands;

public record CreateCompanyCommand(CompanyDto Model) : IRequest<ApplicationResult<CompanyCreatedDto, ApplicationError>>;


public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator(IRepository<Company> repository)
    {
        RuleFor(x => x.Model.CompanyName)
            .NotNull()
            .NotEmpty()
            .MustAsync(async (name, cancellationToken) =>
            {
                return !await repository.Query(comp => comp.Name == name).AnyAsync(cancellationToken);
            })
            .WithMessage("company with this name already exists");
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
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publisher;
    public CreateCompanyCommandHandler(IRepository<Company> repository, IUnitOfWork unitOfWork, IPublishEndpoint publisher)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task<ApplicationResult<CompanyCreatedDto, ApplicationError>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var apiKey = SecurityService.GenerateApiKey(request.Model.CompanyName);
        var secret = SecurityService.GenerateApiSecret();

        var (hash, salt) = SecurityService.GenerateHash(secret);
        var model = request.Model;
        var company = Company.CreateCompany(model.CompanyName, model.City, model.Email, apiKey, hash, salt);
        await _repository.Store(company);
        await _unitOfWork.SaveAsync();
        return new ApplicationResult<CompanyCreatedDto, ApplicationError>(
            new CompanyCreatedDto(company.Id, company.Name,
                company.City, company.Email, company.ApiKey,
                secret, company.CreateDate));
    }
}