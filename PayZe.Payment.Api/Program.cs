using Microsoft.Extensions.Options;
using PayZe.Payment.Api;
using PayZe.Payment.Api.Middlewares;
using PayZe.Payment.Application;
using PayZe.Payment.Infrastructure;
using PayZe.Payment.RabbitMq;
using PayZe.Shared;
using PayZe.Shared.Settings;
using System.Reflection;
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
    options.AddPolicy("fixed-by-user", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name?.ToString(),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1)
            }));
});
var app = builder.Build();

var x = app.Services.GetRequiredService<IOptions<EnvironmentSettings>>();
PrintProperties(x.Value);

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseCors("AllowOrigin");
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
app.UseRateLimiter();
app.UseHttpsRedirection();

app.MapControllers();


app.Run();

static void PrintProperties(object obj)
{
    Type type = obj.GetType();
    PropertyInfo[] properties = type.GetProperties();

    foreach (PropertyInfo property in properties)
    {
        var value = property.GetValue(obj);
        Console.WriteLine($"{property.Name}: {value}");
    }
}