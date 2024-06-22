using Microsoft.AspNetCore.Mvc;
using PayZe.Orders.Api.Abstractions;
using PayZe.Orders.Application.Commands.OrdersCommands;
using PayZe.Orders.Application.Dtos.OrdersDtos;

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
    public async Task<IActionResult> CreateCompanyAsync([FromBody] CreateOrderDto request)
    {

        return (await _mediator.Send(new CreateOrderCommand(
            new OrderDto(CompanyId!.Value, request.Currency, request.Amount)))).Match(Ok, error => BadRequest(error) as IActionResult);
    }
}