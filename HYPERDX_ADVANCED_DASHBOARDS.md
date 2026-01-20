# Advanced HyperDX Dashboards Guide

This guide covers how to set up advanced monitoring for API traffic, including per-account analysis and time-based comparisons.

---

## 1. Monitoring Requests Per Second (RPS)
RPS is a critical metric for understanding your API's load.

### Step-by-Step API Metrics Chart:
1.  Go to **Dashboards** -> **API Performance Monitoring**.
2.  Click **Add Tile** -> **Chart**.
3.  **Title**: `Total Requests Per Second`.
4.  **Metric**: `http.server.request.duration_count`.
5.  **Aggregation**: `Rate` (this automatically converts the total count into a "per second" value).
6.  **Granularity**: `1s` or `Auto`.
7.  **Chart Type**: Area Chart.
8.  Click **Save**.

### Alternative (Log-based RPS):
If you want to see RPS based on specific log patterns (like "Weather Forecast Generated"):
1.  **Metric**: `logs.count`.
2.  **Filter**: `message: "Generated * forecasts"`.
3.  **Aggregation**: `Rate`.

---

## 2. Top Requests by Account ID
Now that the API is instrumented with `app.account_id`, we can see which users are hitting our system the most.

### Step-by-Step Top Accounts Chart:
1.  Click **Add Tile** -> **Chart**.
2.  **Title**: `Requests by Account (Top 5)`.
3.  **Metric**: `http.server.request.duration_count`.
4.  **Aggregation**: `Rate`.
5.  **Group By**: `app.account_id`.
6.  **Chart Type**: Bar Chart or Stacked Area Chart.
7.  **Limit**: `5`.
8.  Click **Save**.

*Note: You can now see which specific account IDs (e.g., `ACC-101`, `ACC-102`) are responsible for the most traffic.*

---

## 3. Compare with Last Day (Period-over-Period)
Comparing current traffic to the previous day helps identify unusual spikes or drops.

### Step-by-Step Comparison:
1.  HyperDX allows you to overlay historical data on any time-series chart.
2.  Open the settings for your **Requests Per Second** chart.
3.  Look for the **"Compare to"** or **"Time Offset"** option.
4.  Select `1 day ago`.
5.  The chart will now show:
    *   **Solid Line**: Current traffic.
    *   **Dashed/Dotted Line**: Traffic from exactly 24 hours ago.
6.  **Benefit**: If your current line is significantly higher than the dashed line, it indicates an unexpected traffic spike.

---

## 4. Advanced: P99 Latency per Account
Combining multiple concepts to find "Slow experience for specific accounts":

1.  **Metric**: `http.server.request.duration_ms`.
2.  **Aggregation**: `Percentile (99)`.
3.  **Group By**: `app.account_id`.
4.  **Filter**: `http.status_code: 200`.
5.  **Insight**: This shows you if one particular account is experiencing slow response times while others are fast.

---

## Technical Summary of Instrumentation
To enable these charts, we added the following to our .NET API:

| Feature | Implementation | Attribute Name |
| :--- | :--- | :--- |
| **Account ID** | Middleware extracting `X-Account-Id` header | `app.account_id` |
| **Trace Tag** | `Activity.Current.SetTag` | Visible in Traces |
| **Log Scope** | `logger.BeginScope` | Visible in all Logs within the request |
