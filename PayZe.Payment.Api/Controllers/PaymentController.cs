using Microsoft.AspNetCore.Mvc;
using PayZe.Orders.Application.Commands.OrdersCommands;
using PayZe.Orders.Application.Dtos.OrdersDtos;
using PayZe.Payment.Api.Abstractions;

namespace PayZe.Payment.Api.Controllers;

[Route("api/payment")]

public class PaymentController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public PaymentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompanyAsync([FromBody] OrderDto request)
    {

        return (await _mediator.Send(new CreateOrderCommand(request))).Match(Ok, error => BadRequest(error) as IActionResult);
    }
}