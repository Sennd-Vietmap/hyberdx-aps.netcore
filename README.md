# ClickStack Demo: HyperDX + OpenTelemetry + ClickHouse on .NET

This repository demonstrates a full observability stack using:
- **Application**: ASP.NET Core Web API 8/9
- **Instrumentation**: OpenTelemetry (Logs, Metrics, Traces)
- **Backend/UI**: HyperDX (Open Source)
- **Data Store**: ClickHouse

## Prerequisites
- Docker & Docker Compose
- .NET 8 or 9 SDK

## Getting Started

### 1. Start the Infrastructure
Depending on your hardware, this might take a moment to pull images and start.
```powershell
docker compose up -d
```
> **Note**: This starts the "All-in-One" HyperDX stack.
> - **UI**: http://localhost:8080
> - **OTLP Collector**: http://localhost:4318 (HTTP), 4317 (gRPC)

### 2. Run the Application
The application includes a `TrafficGeneratorService` that will automatically start simulating traffic and errors once the app is running.

```powershell
dotnet run --project ClickStack.Api
```

### 3. Verify in HyperDX
1. Open [http://localhost:8080](http://localhost:8080).
2. Create an account (local instance).
3. Connect the data set (it should auto-detect OTel signals).
4. Go to **Logs** or **Traces** to see the `weatherforecast` calls.
   - Look for `service.name: clickstack-demo-api`.
   - You should see correlated traces between the BackgroundService and the Controller.

## Architecture
- **Program.cs**: Registers `TrafficGeneratorService` and `AddClickStackConnect`.
- **Extensions/ClickStackExtensions.cs**: Centralized OTel configuration.
- **Services/TrafficGeneratorService.cs**: Background worker that hits the API to generate traces.

