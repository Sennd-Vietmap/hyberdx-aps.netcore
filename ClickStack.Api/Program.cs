using ClickHouse.ClickStack.AspNetCore;
using ClickStack.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add ClickStack (OpenTelemetry) Observability
builder.AddClickStack(options =>
{
    options.ServiceName = "clickstack-demo-api";
    options.AdditionalSources.Add("ClickStack.TrafficGenerator");
    options.AdditionalSources.Add("ClickStack.Api.Data");
});

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IWeatherRepository, WeatherRepository>();
builder.Services.AddHostedService<ClickStack.Api.Services.TrafficGeneratorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable ClickStack Middleware (Account Tracking, etc.)
app.UseClickStack();

app.MapGet("/weatherforecast", async (IWeatherRepository repository, ILogger<Program> logger) =>
{
    logger.LogInformation("Requesting weather from repository...");
    
    var forecast = await repository.GetForecastsAsync();

    logger.LogInformation("Returning {Count} forecasts to client", forecast.Count());
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();
