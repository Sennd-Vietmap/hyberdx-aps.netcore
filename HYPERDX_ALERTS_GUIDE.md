# HyperDX Alerting Guide

This guide explains how to set up and manage alerts in HyperDX to proactively monitor your API for errors and performance issues.

---

## 1. Creating Alerts from Logs (Log-based Alerts)
Use log-based alerts to be notified immediately when specific error messages or patterns appear in your logs.

### Step-by-Step:
1.  Go to the **Search** tab.
2.  Enter your error query, for example: `service.name: clickstack-demo-api level: error`.
3.  Click the **Alerts** button (usually at the top right of the search results).
4.  Select **Create Alert from Search**.
5.  **Configure Search Alert**:
    *   **Alert Name**: `API Error Spike`.
    *   **Threshold**: Set the count (e.g., `Above 10`).
    *   **Time Window**: (e.g., `last 5 minutes`).
6.  **Notification**: Choose your destination (Email, Slack, or Webhook).
7.  Click **Create**.

---

## 2. Creating Alerts from Metrics (Metric-based Alerts)
Use metric-based alerts to monitor performance indicators like Latency (p99) or Request Per Second (RPS).

### Step-by-Step:
1.  Open one of your charts (e.g., from your **API Performance Monitoring** dashboard).
2.  Click the three dots `...` in the top right corner of the chart tile.
3.  Select **Create Alert**.
4.  **Configure Metric Alert**:
    *   **Alert Name**: `High p99 Latency Alert`.
    *   **Condition**: `Above`.
    *   **Value**: `500` (meaning 500ms).
    *   **Evaluation Period**: `5 minutes` (to avoid alerting on temporary spikes).
5.  **Preview**: Check the preview chart to see when the alert would have fired in the past.
6.  Click **Create**.

---

## 3. Configuring Alert Destinations
Before your alerts can reach you, you must define where they should be sent.

1.  Navigate to **Team Settings** -> **Alert Destinations** in the sidebar.
2.  **Email**: Enter the email addresses of your team members.
3.  **Slack**: Connect your Slack workspace and select a channel (e.g., `#alerts-api`).
4.  **Webhooks**: Use this to integrate with PagerDuty, Opsgenie, or custom automation scripts.

---

## 4. Managing Active Alerts
1.  Navigate to the **Alerts** tab in the left sidebar.
2.  Here you can see:
    *   **Alert History**: Which alerts fired and when.
    *   **Active Rules**: A list of all your configured alert rules.
    *   **Status**: Toggle alerts On/Off or edit their thresholds.

---

## Example Scenarios for ClickStack Demo
| Alert Type | Query/Metric | Threshold | Why? |
| :--- | :--- | :--- | :--- |
| **Error Rate** | `level: error` | `> 0` | Be notified of ANY crash in the API. |
| **Latency SLI** | `http.server.request.duration_ms (p95)` | `> 100ms` | Detect if the API is becoming sluggish for users. |
| **DDoS/Traffic** | `http.server.request.duration_count (Rate)` | `> 100` | Detect an unusual surge in traffic (or an infinite loop in a client). |
| **Account Abuse** | `Group By: app.account_id` | `> 50` | Alert if a single account is making too many requests. |
