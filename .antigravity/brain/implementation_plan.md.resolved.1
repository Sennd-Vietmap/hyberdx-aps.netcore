# ASP.NET API Telemetry Routing through OTel Collector

This plan describes how to route all telemetry (logs, metrics, traces) from the ASP.NET API through the OpenTelemetry Collector before it reaches HyperDX.

## User Review Required

> [!IMPORTANT]
> To route traffic through the collector, we will move the host port mappings (4317, 4318) from the HyperDX container to the OTel Collector container. This ensures that any application sending to `localhost:4317` (the default) will now hit the collector.

## Proposed Changes

### OpenTelemetry Collector

#### [MODIFY] [otel-collector-config.yaml](file:///d:/study/google-antigravity/clickstack-aspnet/otel-collector-config.yaml)
- Add the `otlp` receiver for both gRPC and HTTP.
- Update `service.pipelines` to include `otlp` in `logs`, `metrics`, and `traces`.
- Ensure the `otlp` exporter is configured for all three signals.

### Infrastructure

#### [MODIFY] [docker-compose.yml](file:///d:/study/google-antigravity/clickstack-aspnet/docker-compose.yml)
- **clickstack-observability**: Remove `4317:4317` and `4318:4318` ports. The collector will handle these on the host.
- **otel-collector**: Add `4317:4317` and `4318:4318` ports to listen for incoming telemetry from the host-bound API.

---

## Verification Plan

### Automated Tests
- None.

### Manual Verification
1.  **Restart Services**: `docker-compose up -d`.
2.  **Run API**: `dotnet run --project ClickStack.Api`.
3.  **Check Collector Logs**: `docker logs clickstack-otel-collector` to see spans/logs being received via OTLP.
4.  **HyperDX UI**:
    - Verify that `clickstack-demo-api` data still appears in Logs, Metrics, and Traces tabs.
    - Verify that the path of the data is now `API -> Collector -> HyperDX`.
