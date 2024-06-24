using PayZe.Payment.Api;
using PayZe.Payment.Api.Middlewares;
using PayZe.Payment.Application;
using PayZe.Payment.Infrastructure;
using PayZe.Payment.RabbitMq;
using PayZe.Shared;

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


app.Run();
