using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ClickHouse.ClickStack.AspNetCore;

public class ClickStackMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ClickStackOptions _options;
    private readonly ILogger<ClickStackMiddleware> _logger;

    public ClickStackMiddleware(RequestDelegate next, ClickStackOptions options, ILogger<ClickStackMiddleware> logger)
    {
        _next = next;
        _options = options;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (_options.EnableAccountTracking)
        {
            var accountId = context.Request.Headers[_options.AccountHeaderName].ToString();
            if (!string.IsNullOrEmpty(accountId))
            {
                // 1. Add to Trace (Activity)
                Activity.Current?.SetTag("app.account_id", accountId);

                // 2. Add to Logs (using ILogger scope)
                using (_logger.BeginScope(new Dictionary<string, object> { ["app.account_id"] = accountId }))
                {
                    await _next(context);
                    return;
                }
            }
        }

        await _next(context);
    }
}
