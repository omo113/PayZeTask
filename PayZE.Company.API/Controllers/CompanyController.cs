using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayZe.Identity.Api.Abstractions;
using PayZe.Identity.Application.Commands.CompanyCommands;
using PayZe.Identity.Application.Dtos.CompanyDtos;
using PayZe.Identity.Application.Queries.CompanyQueries;

namespace PayZe.Identity.Api.Controllers;


[Route("api/company")]
public class CompanyController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public CompanyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// registers the company
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateCompanyAsync([FromBody] CompanyDto request)
    {
        return (await _mediator.Send(new CreateCompanyCommand(request))).Match(Ok, error => BadRequest(error) as IActionResult);
    }

    /// <summary>
    /// retrieves information about the current logged-in company
    /// </summary>
    /// <returns></returns>
    [HttpGet("me")]
    public async Task<IActionResult> AboutMeAsync()
    {
        return (await _mediator.Send(new CompanyDetailsQuery(CompanyId!.Value))).Match(Ok, error => BadRequest(error) as IActionResult);
    }
    /// <summary>
    /// retrieves information about the company
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCompanyDetails([FromRoute] Guid id)
    {
        return (await _mediator.Send(new CompanyDetailsQuery(id))).Match(Ok, error => BadRequest(error) as IActionResult);
    }
}