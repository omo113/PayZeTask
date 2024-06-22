using System.Net;

namespace PayZe.Payment.Api.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var response = context.Response;

            switch (ex)
            {
                case UnauthorizedAccessException:
                    context.RequestServices.GetRequiredService<ILoggerFactory>()
                        .CreateLogger<ErrorHandlerMiddleware>()
                        .LogError(ex, "");
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    break;
                //case RpcException:
                //    context.RequestServices.GetRequiredService<ILoggerFactory>()
                //        .CreateLogger<ErrorHandlerMiddleware>()
                //        .LogError(ex, "");
                //    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                //    break;
                default:
                    context.RequestServices.GetRequiredService<ILoggerFactory>()
                        .CreateLogger<ErrorHandlerMiddleware>()
                        .LogError(ex, "");
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
        }
    }
}