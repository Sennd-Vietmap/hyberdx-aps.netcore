using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ClickHouse.ClickStack.AspNetCore;

public static class ServiceCollectionExtensions
{
    public static IHostApplicationBuilder AddClickStack(this IHostApplicationBuilder builder, Action<ClickStackOptions>? configure = null)
    {
        var options = new ClickStackOptions();
        
        // 1. Bind from configuration first (ClickStack prefix)
        builder.Configuration.GetSection("ClickStack").Bind(options);
        
        // 2. Apply explicit configuration
        configure?.Invoke(options);

        // Define the resource
        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(serviceName: options.ServiceName, serviceVersion: options.ServiceVersion);

        // --- 1. Logging ---
        builder.Logging.AddOpenTelemetry(loggingOptions =>
        {
            loggingOptions.SetResourceBuilder(resourceBuilder);
            loggingOptions.IncludeScopes = true;
            loggingOptions.IncludeFormattedMessage = true;
            loggingOptions.AddOtlpExporter(otlpOptions =>
            {
                otlpOptions.Endpoint = new Uri(options.OtelEndpoint);
                if (!string.IsNullOrEmpty(options.ApiKey))
                {
                    otlpOptions.Headers = $"authorization={options.ApiKey}";
                }
            });
        });

        // --- 2. Metrics & Tracing ---
        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.SetResourceBuilder(resourceBuilder)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddOtlpExporter(otlpOptions =>
                    {
                        otlpOptions.Endpoint = new Uri(options.OtelEndpoint);
                        if (!string.IsNullOrEmpty(options.ApiKey))
                        {
                            otlpOptions.Headers = $"authorization={options.ApiKey}";
                        }
                    });
            })
            .WithTracing(tracing =>
            {
                tracing.SetResourceBuilder(resourceBuilder)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();

                foreach (var source in options.AdditionalSources)
                {
                    tracing.AddSource(source);
                }

                tracing.AddOtlpExporter(otlpOptions =>
                {
                    otlpOptions.Endpoint = new Uri(options.OtelEndpoint);
                    if (!string.IsNullOrEmpty(options.ApiKey))
                    {
                        otlpOptions.Headers = $"authorization={options.ApiKey}";
                    }
                });
            });

        // Register options for middleware access
        builder.Services.AddSingleton(options);

        return builder;
    }
}
