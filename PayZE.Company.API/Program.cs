using PayZe.Identity.Api;
using PayZe.Identity.Api.Middlewares;
using PayZe.Identity.Application;
using PayZe.Identity.Application.Services;
using PayZe.Identity.Infrastructure;
using PayZe.Identity.RabbitMq;
using PayZe.Shared;

var builder = WebApplication.CreateBuilder(args);

//builder.Services
//    .AddTransient<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddCors(c => { c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); });
builder.Services.AddApplication();
builder.Services.AddDbContext(builder.Configuration);
builder
    .Host
    .AddSettingsConfiguration()
    .ConfigureMessageBus();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerSettings();
var app = builder.Build();
app.MapGrpcService<GrpcIdentityService>();
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
