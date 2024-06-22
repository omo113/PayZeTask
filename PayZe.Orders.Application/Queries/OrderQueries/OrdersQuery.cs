using Microsoft.EntityFrameworkCore;
using PayZe.Orders.Application.Dtos.OrdersDtos;
using PayZe.Orders.Domain.Aggregates.OrderAggregate;
using PayZe.Orders.Infrastructure.Repositories.Repositories.Abstractions;
using PayZe.Shared.ApplicationInfrastructure;
using PayZe.Shared.Infrastructure;

namespace PayZe.Orders.Application.Queries.OrderQueries;

public record OrdersQuery(Guid CompanyId, Query Query) : IRequest<ApplicationResult<QueryResult<OderDetailsDto>, ApplicationError>>;


public class OrdersQueryHandler : IRequestHandler<OrdersQuery, ApplicationResult<QueryResult<OderDetailsDto>, ApplicationError>>
{
    private readonly IRepository<Order> _orderRepository;

    public OrdersQueryHandler(IRepository<Order> orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<ApplicationResult<QueryResult<OderDetailsDto>, ApplicationError>> Handle(OrdersQuery request, CancellationToken cancellationToken)
    {
        var skip = request.Query.PageIndex * request.Query.PageSize;
        var take = request.Query.PageSize;
        var query = _orderRepository.Query(x => x.Company.UId == request.CompanyId)
            .Select(x => new OderDetailsDto(x.UId, x.Company.UId, x.Currency, x.Amount, x.CreateDate));
        var data = await query
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken: cancellationToken);
        return new ApplicationResult<QueryResult<OderDetailsDto>, ApplicationError>(
            new QueryResult<OderDetailsDto>
            {
                Result = data,
                TotalSize = await query.CountAsync(cancellationToken: cancellationToken)
            });
    }
}