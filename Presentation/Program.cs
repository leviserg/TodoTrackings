using Application.Commands;
using Application.DTOs;
using Application.Queries;
using Infrastructure;
using MediatR;
using Presentation;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var configuration = builder.Configuration;

builder.Services.AddInfrastructure(configuration);

builder.Services.AddMediatR(
    cfg => cfg.RegisterServicesFromAssembly(typeof(TodoTaskDto).Assembly)
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region weatherforecast
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();
#endregion

#region POST: createTodoTask

app.MapPost("/tasks", async (CreateTodoTaskCommand command, IMediator mediator) =>
{
    var id = await mediator.Send(command);
    return Results.Created($"/tasks/{id}", id);
})
.WithName("PostNewTodoItem")
.WithOpenApi();

#endregion

#region GET: getTodoTaskById

app.MapGet("/tasks/{id:guid}", async (Guid id, IMediator mediator) =>
{
    var task = await mediator.Send(new GetTodoTaskByIdQuery { Id = id });
    return task is null ? Results.NotFound() : Results.Ok(task);
});

#endregion

#region DELETE: deleteTodoTaskById

app.MapDelete("/tasks/{id:guid}", async (Guid id, IMediator mediator) =>
{
    await mediator.Send(new DeleteTodoTaskCommand { Id = id });
    return Results.NoContent();
});

#endregion

#region PUT: deleteTodoTaskById

app.MapPut("/tasks/{id:guid}", async (Guid id, UpdateTodoTaskCommand command, IMediator mediator) =>
{
    if (id != command.Id) return Results.BadRequest();
    var updatedItem = await mediator.Send(command);
    return updatedItem is null ? Results.NotFound() : Results.Ok(updatedItem);
});

#endregion

await app.RunAsync();

namespace Presentation
{
    internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
