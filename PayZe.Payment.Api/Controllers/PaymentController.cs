using Microsoft.AspNetCore.Mvc;
using PayZe.Payment.Api.Abstractions;
using PayZe.Payment.Application.Commands.PaymentCommands;
using PayZe.Payment.Application.Dtos.PaymentDtos;

namespace PayZe.Payment.Api.Controllers;

[Route("api/payment")]

public class PaymentController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public PaymentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{orderId:guid}")]
    public async Task<IActionResult> CreateCompanyAsync([FromRoute] Guid orderId, PaymentDto model)
    {

        return (await _mediator.Send(new CreatePaymentCommand(orderId, model))).Match(Ok, error => BadRequest(error) as IActionResult);
    }
}