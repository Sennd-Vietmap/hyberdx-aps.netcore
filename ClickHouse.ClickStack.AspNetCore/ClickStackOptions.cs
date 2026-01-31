namespace ClickHouse.ClickStack.AspNetCore;

public class ClickStackOptions
{
    /// <summary>
    /// The name of the service for identification.
    /// </summary>
    public string ServiceName { get; set; } = "unknown-service";

    /// <summary>
    /// The version of the service.
    /// </summary>
    public string ServiceVersion { get; set; } = "1.0.0";

    /// <summary>
    /// The OTel Collector endpoint (e.g., http://localhost:4317).
    /// </summary>
    public string OtelEndpoint { get; set; } = "http://localhost:4317";

    /// <summary>
    /// Optional API key for ingestion authentication.
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// Whether to enable automatic account tracking via middleware.
    /// </summary>
    public bool EnableAccountTracking { get; set; } = true;

    /// <summary>
    /// The name of the header containing the Account ID.
    /// </summary>
    public string AccountHeaderName { get; set; } = "X-Account-Id";

    /// <summary>
    /// Additional sources for tracing (e.g., custom ActivitySource names).
    /// </summary>
    public List<string> AdditionalSources { get; } = new();
}
