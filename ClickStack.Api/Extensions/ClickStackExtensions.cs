using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ClickStack.Api.Extensions;

public static class ClickStackExtensions
{
    public static IHostApplicationBuilder AddClickStackConnect(this IHostApplicationBuilder builder)
    {
        var serviceName = "clickstack-demo-api";
        var otelEndpoint = builder.Configuration["ClickStack:OtelEndpoint"] ?? "http://localhost:4317";

        // Define the resource (service identification)
        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(serviceName: serviceName, serviceVersion: "1.0.0");

        // generic OTLP configured via code for explicit clarity in demo
        // In prod, env vars (OTEL_EXPORTER_OTLP_ENDPOINT) are often used automatically.
        
        // 1. Logging
        builder.Logging.ClearProviders(); // Optional: remove console if we want *only* structured, but keep for dev
        builder.Logging.AddConsole(); 
        builder.Logging.AddOpenTelemetry(options =>
        {
            options.SetResourceBuilder(resourceBuilder);
            options.IncludeScopes = true;
            options.IncludeFormattedMessage = true;
            options.AddOtlpExporter(otlpOptions =>
            {
                otlpOptions.Endpoint = new Uri(otelEndpoint);
            });
        });

        // 2. Metrics
        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.SetResourceBuilder(resourceBuilder)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddOtlpExporter(otlpOptions =>
                    {
                        otlpOptions.Endpoint = new Uri(otelEndpoint);
                    });
            })
        // 3. Tracing
            .WithTracing(tracing =>
            {
                tracing.SetResourceBuilder(resourceBuilder)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSource("ClickStack.TrafficGenerator")
                    .AddOtlpExporter(otlpOptions =>
                    {
                        otlpOptions.Endpoint = new Uri(otelEndpoint);
                    });
            });

        return builder;
    }
}
