# ClickStack: Reliability Engineering Guide (SLO/SLI)

This guide helps you transition from basic monitoring to **Site Reliability Engineering (SRE)** principles using your ClickStack observability data.

---

## 1. Defining SLIs & SLOs

### Service Level Indicators (SLIs)
These are quantitative measures of your service's reliability.
- **Availability**: Percentage of successful requests (StatusCode < 500).
- **Latency**: Time taken to fulfill a request (P95 or P99).
- **Correctness**: Percentage of responses with the expected payload.

### Service Level Objectives (SLOs)
These are target values for your SLIs over a specific time window.
- **Availability SLO**: 99.5% uptime over 30 days.
- **Latency SLO**: 95% of requests < 300ms over 7 days.

---

## 2. Implementing SLI Dashboards with SQL

You can use raw SQL in HyperDX to track your SLOs in real-time.

### Availability SLI Query
```sql
SELECT 
    countIf(statusCode < 500) / count() * 100 as availability_percentage
FROM spans
WHERE _time > now() - INTERVAL 30 DAY
```

### Latency (P99) SLI Query
```sql
SELECT 
    quantile(0.99)(durationNano) / 1000000 as p99_latency_ms
FROM spans
WHERE _time > now() - INTERVAL 7 DAY
```

---

## 3. Automated Alerting Strategy

HyperDX provides a built-in alerting engine that can notify your team when SLIs are breached.

### Recommended Alert Configurations:
1. **High Error Rate**: Trigger if `availability_percentage < 99.0%` for more than 5 minutes.
2. **Latency Spike**: Trigger if `p99_latency_ms > 1000ms` for more than 2 minutes.
3. **Log Flood**: Trigger if log volume increases by >300% in a 1-minute window.

### Integration Steps:
1. Navigate to **Alerts** in the HyperDX UI.
2. Click **Create New Alert**.
3. Choose **SQL Alert** or **Metric Alert**.
4. Configure **Slack**, **PagerDuty**, or a **Webhook** as the notification destination.

---

## 4. Log Pattern Recognition

When an incident occurs, use the **Patterns** feature in the HyperDX Logs tab.
- **Identify Noise**: Group 1,000,000 logs into 5 distinct patterns.
- **Drill Down**: See how an error pattern correlates with specific `app.account_id` tags.
- **Trend Analysis**: See if a new code version introduced a specific log pattern.

---

## 5. Error Budgets
An Error Budget is `1 - SLO`. For an availability SLO of 99.5%, your error budget is 0.5%.
- **Budget Consumed**: If your API is down for 3 hours, you've consumed a portion of your budget.
- **Policy**: If the budget is exhausted, stop shipping new features and focus exclusively on stability.
