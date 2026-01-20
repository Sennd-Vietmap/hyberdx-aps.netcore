using System.Diagnostics;

namespace ClickStack.Api.Services;

public class TrafficGeneratorService : BackgroundService
{
    private readonly ILogger<TrafficGeneratorService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly Random _random = new();

    public TrafficGeneratorService(ILogger<TrafficGeneratorService> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Traffic Generator started via BackgroundService.");
        
        // Wait for server to boot
        await Task.Delay(5000, stoppingToken);

        using var client = _httpClientFactory.CreateClient();
        // Since we are running in the same process, we need the local address.
        // In dev, usually http://localhost:5000 or defined in launchSettings.
        // We will try standard ports.
        var targetUrl = "http://localhost:5066/weatherforecast"; 
        
        while (!stoppingToken.IsCancellationRequested)
        {
            using var activity = new ActivitySource("ClickStack.TrafficGenerator").StartActivity("SimulateTraffic");
            
            try
            {
                activity?.SetTag("simulation.iteration", DateTime.UtcNow.Ticks);
                _logger.LogInformation("Simulating background processing job...");

                // 1. Simulate HTTP Traffic (which creates its own traces automaticall via OTel HttpClient instrumentation)
                try 
                {
                    var response = await client.GetAsync(targetUrl, stoppingToken);
                    activity?.SetTag("http.status_code", (int)response.StatusCode);
                }
                catch(Exception httpEx)
                {
                     _logger.LogWarning("Could not hit local API: {Message}", httpEx.Message);
                }
                
                // 2. Simulate Manual Work/Latency
                await Task.Delay(_random.Next(500, 2000), stoppingToken);
                
                if (_random.Next(10) > 8)
                {
                    _logger.LogError("Random simulated error occurred!");
                    activity?.SetStatus(ActivityStatusCode.Error, "Simulated Random Error");
                }
                else
                {
                     activity?.SetStatus(ActivityStatusCode.Ok);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in traffic generator loop");
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            }
        }
    }
}
