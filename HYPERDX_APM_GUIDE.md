# HyperDX APM (Application Performance Monitoring) Guide

This guide covers how to use the full APM capabilities of HyperDX to monitor, debug, and optimize your .NET application performance.

---

## 1. The Service Inventory
The Service Inventory is the "home base" for APM. It provides a high-level health check of all your instrumented services.

### Step-by-Step:
1.  Navigate to the **Services** tab in the left sidebar.
2.  Find `clickstack-demo-api` in the list.
3.  Observe the key metrics at a glance:
    *   **Latency (p50, p95, p99)**: The speed of your API.
    *   **Throughput (RPS)**: The volume of requests.
    *   **Error Rate**: The percentage of failed requests.
4.  **Insight**: If the Error Rate turns red or p99 spikes, you know exactly which service needs attention.

---

## 2. Distributed Tracing & Span Analysis
Tracing is the core of APM. It allows you to see the "path" of a request across your system.

### Step-by-Step:
1.  From the **Services** list, click on `clickstack-demo-api`.
2.  In the **Operations** list, find `GET /weatherforecast`.
3.  Click on any individual request in the **Trace List**.
4.  The **Trace View** shows:
    *   **Timeline (Gantt Chart)**: Which part of the request took the longest?
    *   **Tags/Attributes**: View standard OTel attributes AND our custom `app.account_id`.
    *   **Logs**: See logs generated *during this specific request* in chronological order.
5.  **Troubleshooting**: Look for the "Flame Graph" to identify "N+1" query problems or slow external API calls.

---

## 3. Service Map (Topology)
The Service Map visualizes how your services, databases, and external APIs communicate.

### Step-by-Step:
1.  Navigate to the **Service Map** tab.
2.  You will see a graph where:
    *   **Nodes**: represent your services (e.g., `clickstack-demo-api`).
    *   **Edges (Lines)**: represent the traffic flow.
3.  The lines indicate latency and volume between services.
4.  **Action**: Click on a node to see the specific health metrics for that component of your architecture.

---

## 4. Identifying Performance Bottlenecks
APM helps you find "Hot Spots" where your code is slow.

### Step-by-Step:
1.  Go to the **Traces** tab.
2.  Use the filter: `duration: > 100ms` (to find slow requests).
3.  Sort by `Duration (Descending)`.
4.  Open a slow trace and look for the **longest span**.
    *   If the longest span is `GET /weatherforecast`, the logic inside the Controller is slow.
    *   If the longest span is a database call, the database needs an index.
    *   If the longest span is an external HTTP call, the remote service is slow.

---

## 5. Correlating Logs with Performance
One of the most powerful features of HyperDX APM is the seamless jump between signals.

1.  **From Log to Trace**: Clicking the Trace ID in any log entry takes you to the APM trace view.
2.  **From Trace to Metric**: The service dashboard overlays your metrics (CPU/RAM) with your trace performance.
3.  **From Metric to Log**: Clicking a spike in a latency chart will show you the specific logs that occurred during that spike.

---

## Summary of APM Terminology
| Term | Definition | HyperDX location |
| :--- | :--- | :--- |
| **Service** | An application or process (e.g., your API). | Services Tab |
| **Operation** | A specific endpoint or function (e.g., `GET /weatherforecast`). | Operations Detail |
| **Span** | A single unit of work (e.g., a DB call or a method execution). | Inside a Trace |
| **Trace** | A collection of spans representing a single transaction. | Traces Tab |
| **P99** | The 99th percentile (99% of requests are faster than this value). | Service Dashboard |
