using PayZe.Orders.Api;
using PayZe.Orders.Api.Middlewares;
using PayZe.Orders.Application;
using PayZe.Orders.Infrastructure;
using PayZe.Orders.RabbitMq;
using PayZe.Shared;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(c => { c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); });
builder.Services.AddApplication();
builder.Services.AddDbContext(builder.Configuration);
builder
    .Host
    .AddSettingsConfiguration()
    .AddIdentityEnvVariable()
    .ConfigureMessageBus();
builder.Services.AddRedis();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerSettings();

builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("fixed-by-ip", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1)
            }));
});
var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseCors("AllowOrigin");
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.MapControllers();
app.UseRateLimiter();

app.Run();
