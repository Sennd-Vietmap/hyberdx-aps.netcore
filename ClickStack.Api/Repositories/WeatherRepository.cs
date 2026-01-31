using System.Diagnostics;

namespace ClickStack.Api.Repositories;

public interface IWeatherRepository
{
    Task<IEnumerable<WeatherForecast>> GetForecastsAsync();
}

public class WeatherRepository : IWeatherRepository
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private static readonly ActivitySource ActivitySource = new("ClickStack.Api.Data");

    public async Task<IEnumerable<WeatherForecast>> GetForecastsAsync()
    {
        using var activity = ActivitySource.StartActivity("DB Query", ActivityKind.Client);
        
        // --- Service Map Metadata (Conventional Attributes) ---
        activity?.SetTag("db.system", "clickhouse");
        activity?.SetTag("db.name", "weather_db");
        activity?.SetTag("db.statement", "SELECT * FROM forecasts LIMIT 5");
        activity?.SetTag("peer.service", "clickhouse-cluster");

        // --- Custom Span Events (Milestones) ---
        activity?.AddEvent(new ActivityEvent("db.query.execution_started"));

        // Simulate DB Latency
        await Task.Delay(Random.Shared.Next(50, 200));

        var results = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                Summaries[Random.Shared.Next(Summaries.Length)]
            )).ToList();

        activity?.AddEvent(new ActivityEvent("db.data_retrieved", tags: new ActivityTagsCollection 
        { 
            ["db.rows_count"] = results.Count 
        }));

        return results;
    }
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
