using Microsoft.AspNetCore.Builder;

namespace ClickHouse.ClickStack.AspNetCore;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds the ClickStack middleware to the pipeline, enabling automated Account ID tracking and telemetry context.
    /// </summary>
    public static IApplicationBuilder UseClickStack(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ClickStackMiddleware>();
    }
}
