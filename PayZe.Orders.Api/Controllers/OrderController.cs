using Microsoft.AspNetCore.Mvc;
using PayZe.Orders.Api.Abstractions;
using PayZe.Orders.Application.Commands.OrdersCommands;
using PayZe.Orders.Application.Dtos.OrdersDtos;
using PayZe.Orders.Application.Queries.OrderQueries;
using PayZe.Shared.Infrastructure;

namespace PayZe.Orders.Api.Controllers;

[Route("api/order")]
public class OrderController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderDto request)
    {

        return (await _mediator.Send(new CreateOrderCommand(
            new OrderDto(CompanyId!.Value, request.Currency, request.Amount)))).Match(Ok, error => BadRequest(error) as IActionResult);
    }
    [HttpGet]
    public async Task<IActionResult> GetOrdersAsync([FromQuery] Query query)
    {
        return (await _mediator.Send(new OrdersQuery(CompanyId!.Value, query))).Match(Ok, error => BadRequest(error) as IActionResult);
    }
    [HttpGet("compute")]
    public async Task<IActionResult> CalculateCompletedSum()
    {
        return (await _mediator.Send(new CompletedOrdersTotalAmountQuery(CompanyId!.Value))).Match(Ok, error => BadRequest(error) as IActionResult);
    }
}