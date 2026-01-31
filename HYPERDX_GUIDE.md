# ClickStack (HyperDX) Observability Guide

This guide provides step-by-step instructions for exploring your telemetry data and setting up monitoring dashboards in the ClickStack (powered by HyperDX) environment.

## 1. Viewing Logs
The ClickStack environment aggregates logs from your .NET API via the OpenTelemetry collector.

1.  Open the HyperDX UI at [http://localhost:8686](http://localhost:8686).
2.  Navigate to the **Search** (Magnifying glass) tab.
3.  In the search bar, enter: `service.name: clickstack-demo-api`.
4.  You will see logs such as:
    *   `Getting weather forecast at...`
    *   `Generated 5 forecasts`
    *   `Random simulated error occurred!` (marked with a red error indicator).
5.  Click on any log line to see the full set of attributes and the correlated **Span ID**.

## 2. Viewing Traces
Traces allow you to see the end-to-end journey of a request.

1.  In the **Search** tab, click the **Traces** toggle at the top (usually next to "Logs").
2.  Search for `service.name: clickstack-demo-api`.
3.  Click on a trace (e.g., `GET /weatherforecast`).
4.  You will see a Gantt chart showing:
    *   The incoming HTTP request.
    *   Internal activities (if any).
    *   Correlated logs within context.
5.  Notice how the `TrafficGeneratorService` creates traces that span across the background worker and the API controller.

## 3. Viewing Metrics
Metrics provide a high-level overview of system health.

1.  Navigate to the **Metrics** tab on the left sidebar.
2.  You can browse available metrics like:
    *   `http_server_duration_ms_bucket`: Histogram of request durations.
    *   `http_server_active_requests`: Current number of active requests.

---

## 4. Creating a Monitoring Dashboard
We will create a dashboard to track **p95, p99 Latency** and **Requests Per Second (RPS)**.

### Step 4.1: Create a Dashboard
1.  Navigate to **Dashboards** in the left sidebar.
2.  Click **+ New Dashboard**.
3.  Name it: `API Performance Monitoring`.

### Step 4.2: Add p99 and p95 Latency Chart
1.  Click **Add Tile** -> **Chart**.
2.  **Chart Type**: Line Chart.
3.  **Metrics Query**:
    *   Select metric: `http.server.request.duration_ms` (or similar depending on OTel version).
    *   **Aggregation**: `Percentile`.
    *   **Percentile value**: `99`.
    *   **Group by**: `http.route` (optional).
4.  Click **Add Metric** (to add p95 to the same chart).
    *   **Aggregation**: `Percentile`.
    *   **Percentile value**: `95`.
5.  **Title**: `API Latency (p95 & p99)`.
6.  Click **Save**.

### Step 4.3: Add Request Per Second (RPS) Chart
1.  Click **Add Tile** -> **Chart**.
2.  **Chart Type**: Area Chart.
3.  **Metrics Query**:
    *   Select metric: `http.server.request.duration_count` (this represents the number of requests).
    *   **Aggregation**: `Rate` (or `Sum` with a `1s` time interval).
4.  **Title**: `Requests Per Second`.
5.  Click **Save**.

---

## Tips for HyperDX
*   **Correlation**: When looking at a log, the "Trace ID" link will take you directly to the trace that generated that log.
*   **Alerts**: You can create alerts on any dashboard chart by clicking the three dots `...` on the tile and selecting **Create Alert**.
