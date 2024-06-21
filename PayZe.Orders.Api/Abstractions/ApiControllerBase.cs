using Microsoft.AspNetCore.Mvc;
using PayZe.Orders.Api.Middlewares;

namespace PayZe.Orders.Api.Abstractions;


[ApiController]
[TypeFilter(typeof(AuthorizationFilter))]
public class ApiControllerBase : ControllerBase
{
    public Guid? CompanyId => HttpContext.Request.Headers["CompanyId"][0] != null
        ? Guid.Parse(HttpContext.Request.Headers["CompanyId"][0]!)
        : null;
}