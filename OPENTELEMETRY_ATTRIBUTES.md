# OpenTelemetry Attributes & Semantic Conventions Guide

This guide explains how to use Attributes in OpenTelemetry to enrich your telemetry data (Traces, Logs, and Metrics), with a focus on .NET implementation and best practices.

---

## 1. Core Concepts: What are Attributes?

Attributes are key-value pairs that provide metadata to your telemetry signals. They are the primary way to add context to your data, enabling powerful filtering and grouping in tools like HyperDX.

### Signal Types
*   **Span Attributes**: Metadata for a specific operation. (e.g., `user.id`, `order.total`).
*   **Resource Attributes**: Metadata about the entity (e.g., `service.name`, `deployment.environment`, `host.name`). Usually set at startup.
*   **Log Attributes**: Structured fields in your logs. (e.g., `request.path`, `error.code`).

---

## 2. Semantic Conventions (Standardized Naming)

OpenTelemetry defines **Semantic Conventions** to ensure consistency across different languages and tools. **Always prefer these over custom names when applicable.**

| Namespace | Example Attributes | Description |
| :--- | :--- | :--- |
| **HTTP** | `http.method`, `http.status_code`, `http.url` | Web request details |
| **Database** | `db.system`, `db.user`, `db.statement` | Database operation details |
| **Service** | `service.name`, `service.version` | Identifying the application |
| **Host** | `host.name`, `host.id`, `host.type` | Infrastructure details |
| **Exception** | `exception.type`, `exception.stacktrace` | Error details |

---

## 3. Implementation in .NET (C#)

### Adding Span Attributes (Tags)
In .NET, Spans are represented by `System.Diagnostics.Activity`.

```csharp
using var activity = MyActivitySource.StartActivity("ProcessPayment");

if (activity != null)
{
    // Add custom attribute (Tag)
    activity.SetTag("payment.provider", "stripe");
    activity.SetTag("payment.amount", 49.99);
}
```

### Adding Resource Attributes
Configured during the initialization of the Tracer or Logger provider.

```csharp
var resourceBuilder = ResourceBuilder.CreateDefault()
    .AddService("ClickStack.Api")
    .AddAttributes(new Dictionary<string, object>
    {
        ["deployment.environment"] = "staging",
        ["region"] = "us-east-1"
    });

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing
        .SetResourceBuilder(resourceBuilder)
        // ...
    );
```

### Adding Log Attributes
Use `BeginScope` or structured logging.

```csharp
using (_logger.BeginScope(new Dictionary<string, object> { ["UserId"] = 123 }))
{
    _logger.LogInformation("Processing user request");
}
```

---

## 4. Best Practices

### Naming
*   **Lowercase**: Use `user.id`, not `User_Id`.
*   **Namespace with Dots**: Use `app.feature.name`.
*   **Be Specific**: Use `disk.free_space` instead of `free`.

### Cardinality (The "Too Many Unique Values" Problem)
*   **Traces**: High cardinality is **GOOD**. Add specific IDs like `order_id` or `user_id`.
*   **Metrics**: High cardinality is **BAD**. Never add unique IDs to metrics, as it creates too many time-series and slows down the database. Use buckets or categories instead.

### Security
*   **NEVER** add PII (Personally Identifiable Information) or secrets (passwords, API keys, CC numbers) to attributes.
*   Telemetry is often stored for long periods and may be visible to many team members.
