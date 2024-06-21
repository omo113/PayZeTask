using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayZe.Identity.Application.Commands.CompanyCommands;
using PayZe.Identity.Application.Dtos.CompanyDtos;
using PayZe.Identity.Application.Queries.CompanyQueries;
using PayZe.Shared;

namespace PayZe.Identity.Api.Controllers;


[Route("api/company")]
[ApiController]
public class CompanyController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ApplicationContext _context;

    public CompanyController(IMediator mediator, ApplicationContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateCompanyAsync([FromBody] CompanyDto request)
    {
        return (await _mediator.Send(new CreateCompanyCommand(request))).Match(Ok, error => BadRequest(error) as IActionResult);
    }

    [HttpGet]
    public async Task<IActionResult> AboutMeAsync()
    {
        return (await _mediator.Send(new CompanyDetailsQuery(_context.RequestingCompanyUId!.Value))).Match(Ok, error => BadRequest(error) as IActionResult);
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCompanyDetails([FromQuery] Guid id)
    {
        return (await _mediator.Send(new CompanyDetailsQuery(id))).Match(Ok, error => BadRequest(error) as IActionResult);
    }
}