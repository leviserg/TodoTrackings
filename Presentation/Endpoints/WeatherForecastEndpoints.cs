using Application.Commands;
using Application.Queries;
using System.Collections.ObjectModel;

namespace Presentation.Endpoints;

public static class WeatherForecastEndpoints
{
    public static RouteGroupBuilder MapWeatherForecastEndpoints(this RouteGroupBuilder group)
    {

        group.MapGet("/", () =>
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Count)]
                ))
                .ToArray();
            return forecast;
        })
        .WithName("GetWeatherForecast")
        .WithOpenApi();

        return group;
    }

    private static ReadOnlyCollection<string> summaries = new List<string>
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    }.AsReadOnly();
}

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
