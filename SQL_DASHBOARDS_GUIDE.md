# ClickStack: SQL-Based Dashboards Guide

ClickStack (HyperDX) allows you to bypass the standard metrics UI and run raw **ClickHouse SQL queries** to build highly customized dashboards.

---

## 1. Why use SQL for Dashboards?
While OTel metrics are great, SQL allows you to:
- Perform complex joins between logs and traces.
- Aggregate data with custom logic (e.g., "Median Latency for Premium Users").
- Filter data using regular expressions or subqueries.

## 2. Accessing the SQL Editor
1. Open HyperDX at [http://localhost:8686](http://localhost:8686).
2. Go to the **Dashboards** tab.
3. Click **Add Tile** -> **Chart**.
4. Instead of choosing a Metric, look for the **"SQL"** or **"Raw Query"** toggle/tab in the chart editor.

## 3. Useful ClickStack SQL Queries

### A. Total Rows Ingested by Service
```sql
SELECT 
    serviceName, 
    count() as total_rows
FROM logs
WHERE _time > now() - INTERVAL 1 HOUR
GROUP BY serviceName
ORDER BY total_rows DESC
```

### B. Average Latency for specific DB Operations
Since we added `db.system` tags, we can measure our simulated database performance:
```sql
SELECT 
    spanName,
    avg(durationNano) / 1000000 as avg_duration_ms
FROM spans
WHERE spanAttributes['db.system'] = 'clickhouse'
  AND _time > now() - INTERVAL 30 MINUTE
GROUP BY spanName
```

### C. Error Rate by Account ID
```sql
SELECT 
    spanAttributes['app.account_id'] as account_id,
    countIf(statusCode = 2) / count() * 100 as error_percentage
FROM spans
WHERE account_id IS NOT NULL
GROUP BY account_id
```

---

## 4. Visualizing SQL Results
Once your query returns data:
1. **X-Axis**: Map this to your time column (e.g., `_time`) or a category (e.g., `serviceName`).
2. **Y-Axis**: Map this to your aggregate value (e.g., `avg_duration_ms`).
3. **Chart Type**: Choose Line, Bar, or Stat based on the data.

> [!TIP]
> Use the [ClickHouse Clustering Guide](./CLICKHOUSE_CLUSTERING_GUIDE.md) to understand how these queries scale when running on multiple shards.
