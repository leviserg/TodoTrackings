using Application.DTOs;
using HealthChecks.UI.Client;
using Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Presentation;
using Presentation.Endpoints;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var configuration = builder.Configuration;

builder.Services.AddInfrastructure(configuration);

builder.Services.AddMediatR(
    cfg => cfg.RegisterServicesFromAssembly(typeof(TodoTaskDto).Assembly)
);

builder.Services.AddDependentServices(configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHealthChecks("/_health", new HealthCheckOptions
{
    ResponseWriter = app.Environment.IsDevelopment() ? UIResponseWriter.WriteHealthCheckUIResponse : UIResponseWriter.WriteHealthCheckUIResponseNoExceptionDetails
});

app.UseHttpsRedirection();

app.MapGroup("/api/todo-tasks").MapTodoTasksEndpoints();
app.MapGroup("/api/weatherforecast").MapWeatherForecastEndpoints();

await app.RunAsync();
