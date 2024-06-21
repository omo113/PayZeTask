using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PayZe.Identity.Application.Dtos.CompanyDtos;
using PayZe.Identity.Domain.Aggregates;
using PayZe.Identity.Infrastructure.Repositories.Repositories.Abstractions;
using PayZe.Shared.ApplicationInfrastructure;

namespace PayZe.Identity.Application.Queries.CompanyQueries;

public record CompanyDetailsQuery(Guid Id) : IRequest<ApplicationResult<CompanyDetailsDto, ApplicationError>>;

public class CompanyDetailsQueryValidator : AbstractValidator<CompanyDetailsQuery>
{
    public CompanyDetailsQueryValidator(IRepository<Company> repository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .MustAsync(async (uid, y) =>
            {
                return await repository.Query(comp => comp.UId == uid).AnyAsync();
            })
            .WithMessage("this kind of company does not exist");
    }
}

public class CompanyDetailsQueryHandler : IRequestHandler<CompanyDetailsQuery, ApplicationResult<CompanyDetailsDto, ApplicationError>>
{
    private readonly IRepository<Company> _repository;

    public CompanyDetailsQueryHandler(IRepository<Company> repository)
    {
        _repository = repository;
    }

    public async Task<ApplicationResult<CompanyDetailsDto, ApplicationError>> Handle(CompanyDetailsQuery request, CancellationToken cancellationToken)
    {
        var company = await _repository.Query(x => x.UId == request.Id)
            .Select(x => new CompanyDetailsDto(x.UId, x.Name, x.City, x.Email, x.CreateDate))
            .FirstAsync(cancellationToken: cancellationToken);
        return new ApplicationResult<CompanyDetailsDto, ApplicationError>(company);
    }
}