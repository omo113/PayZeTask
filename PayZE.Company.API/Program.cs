using PayZe.Identity.Api.Middlewares;
using PayZe.Identity.Application;
using PayZe.Identity.Infrastructure;
using PayZe.Identity.RabbitMq;
using PayZe.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(c => { c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); });
builder
    .Host
    .AddSettingsConfiguration()
    .ConfigureMessageBus();

builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseCors("AllowOrigin");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
