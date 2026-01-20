using System.Diagnostics;
using ClickStack.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add ClickStack (OpenTelemetry) Observability
builder.AddClickStackConnect();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddHostedService<ClickStack.Api.Services.TrafficGeneratorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Middleware to extract Account ID for Observability
app.Use(async (context, next) =>
{
    var accountId = context.Request.Headers["X-Account-Id"].ToString();
    if (!string.IsNullOrEmpty(accountId))
    {
        // Add to Trace (Activity)
        var activity = Activity.Current;
        activity?.SetTag("app.account_id", accountId);

        // Add to Logs (using ILogger scope)
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        using (logger.BeginScope(new Dictionary<string, object> { ["app.account_id"] = accountId }))
        {
            await next();
        }
    }
    else
    {
        await next();
    }
});

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", (ILogger<Program> logger) =>
{
    logger.LogInformation("Getting weather forecast at {Time}", DateTime.UtcNow);
    
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

    logger.LogInformation("Generated {Count} forecasts", forecast.Length);
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
