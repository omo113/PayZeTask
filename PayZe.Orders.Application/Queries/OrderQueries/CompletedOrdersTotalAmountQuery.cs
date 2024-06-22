using Microsoft.EntityFrameworkCore;
using PayZe.Orders.Application.Dtos.CompanyDtos;
using PayZe.Orders.Domain.Aggregates.CompanyAggregate;
using PayZe.Orders.Infrastructure.Repositories.Repositories.Abstractions;
using PayZe.Shared.ApplicationInfrastructure;
using PayZe.Shared.Enums;

namespace PayZe.Orders.Application.Queries.OrderQueries;

public record CompletedOrdersTotalAmountQuery(Guid CompanyId) : IRequest<ApplicationResult<CompanyCompletedOrdersAmountDto, ApplicationError>>;


public class CompletedOrdersTotalAmountQueryHandler : IRequestHandler<CompletedOrdersTotalAmountQuery, ApplicationResult<CompanyCompletedOrdersAmountDto, ApplicationError>>
{
    private readonly IRepository<Company> _repository;

    public CompletedOrdersTotalAmountQueryHandler(IRepository<Company> repository)
    {
        _repository = repository;
    }

    public async Task<ApplicationResult<CompanyCompletedOrdersAmountDto, ApplicationError>> Handle(CompletedOrdersTotalAmountQuery request, CancellationToken cancellationToken)
    {
        var amount = await _repository.Query(x => x.UId == request.CompanyId)
            .Include(x => x.Orders)
            .SelectMany(x => x.Orders)
            .Where(x => x.OrderStatus == OrderStatus.Completed)
            .SumAsync(x => x.Amount, cancellationToken: cancellationToken);
        return new ApplicationResult<CompanyCompletedOrdersAmountDto, ApplicationError>(
            new CompanyCompletedOrdersAmountDto(request.CompanyId, amount));
    }
}