using Microsoft.AspNetCore.Mvc;
using PayZe.Identity.Application.Commands.CompanyCommands;
using PayZe.Identity.Application.Dtos.CompanyDtos;

namespace PayZe.Identity.Api.Controllers;


[Route("api/company")]
[ApiController]
public class CompanyController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompanyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompanyAsync([FromBody] CompanyDto request)
    {
        return (await _mediator.Send(new CreateCompanyCommand(request))).Match(Ok, error => BadRequest(error) as IActionResult);
    }
}