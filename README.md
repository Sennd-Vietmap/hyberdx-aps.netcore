# ClickStack Demo: Official ClickHouse Observability on .NET

This repository demonstrates a production-grade observability stack using:
- **Application**: ASP.NET Core Web API 8/9
- **Instrumentation**: OpenTelemetry (Logs, Metrics, Traces)
- **Stack**: [ClickStack](https://clickhouse.com/docs/use-cases/observability/clickstack) (Official ClickHouse Observability)
- **Data Store**: ClickHouse

## Prerequisites
- Docker & Docker Compose
- .NET 8 or 9 SDK

## Getting Started

### 1. Start the Infrastructure
Depending on your hardware, this might take a moment to pull the official `clickhouse/clickstack-*` images and start.
```powershell
docker compose up -d
```
> **Note**: This starts the **ClickStack "All-in-One"** stack.
> - **Direct UI**: http://localhost:8080
> - **Nginx Proxy UI**: http://localhost:8686
> - **Central OTel Collector**: http://localhost:4318 (HTTP), 4317 (gRPC)

### 2. Run the Application
The application includes a `TrafficGeneratorService` that will automatically start simulating traffic and errors once the app is running. It uses the HyperDX API Key for authenticated OTLP ingestion.

```powershell
dotnet run --project ClickStack.Api
```

### 3. Verify in HyperDX
1. Open [http://localhost:8686](http://localhost:8686) (via Nginx proxy).
2. Create an account (local instance).
3. Connect the data set (it should auto-detect OTel signals).
4. Go to **Logs** or **Traces** to see the `weatherforecast` calls.
   - Look for `service.name: clickstack-demo-api`.
   - You should see correlated traces between the BackgroundService and the Controller.

## Architecture
- **Program.cs**: Registers `TrafficGeneratorService` and `AddClickStackConnect`.
- **Extensions/ClickStackExtensions.cs**: Centralized OTel configuration with `authorization` header support.
- **Services/TrafficGeneratorService.cs**: Background worker that hits the API to generate traces.
- **nginx/nginx.conf**: Reverse proxy configuration for port 8686.
- **appsettings.json**: Stores the `ClickStack:ApiKey` for authentication.

