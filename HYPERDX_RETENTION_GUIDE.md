# HyperDX Data Retention Guide

This guide explains how to configure data retention (TTL - Time To Live) for your self-hosted HyperDX instance using ClickHouse.

---

## 1. Overview of Retention Periods
By default, HyperDX (OSS) typically retains data for **1 month**. To customize this, you must modify the TTL settings directly in the ClickHouse database.

Based on your requirements:
*   **Logs**: 14 Days
*   **Metrics**: 7 Days
*   **Traces (Spans)**: 7 Days

---

## 2. Configuration Steps (ClickHouse TTL)

Since there are currently no environment variables (like `RETENTION_DAYS`) in the HyperDX OSS version, follow these steps to apply the retention policy.

### Step 1: Connect to the ClickHouse Client
Run the following command from your terminal (ensure Docker is running):

```powershell
docker compose exec clickstack-hyperdx clickhouse-client
```
*(Note: Use `clickstack-hyperdx` as it is the name of your service in the current `docker-compose.yml`)*

### Step 2: Execute ALTER TABLE Commands
Run the following SQL commands inside the ClickHouse client:

```sql
-- 1. Set Logs retention to 14 Days
ALTER TABLE log_stream MODIFY TTL toDateTime(_created_at) + INTERVAL 14 DAY DELETE;

-- 2. Set Traces (Spans) retention to 7 Days
ALTER TABLE span_stream MODIFY TTL toDateTime(_created_at) + INTERVAL 7 DAY DELETE;

-- 3. Set Metrics retention to 7 Days
ALTER TABLE metric_stream MODIFY TTL toDateTime(_created_at) + INTERVAL 7 DAY DELETE;
```

### Step 3: Verify the Changes
To confirm that the TTL has been updated successfully, run:

```sql
SELECT 
    name, 
    ttl_table 
FROM system.tables 
WHERE name IN ('log_stream', 'span_stream', 'metric_stream');
```

---

## 3. Important Considerations

*   **Cleanup Frequency**: ClickHouse does not delete expired data instantly. It removes data during background "merge" operations.
*   **Storage Space**: Shorter retention periods will help save disk space on your local machine or server.
*   **No Environment Variables**: Currently, these settings are not persisted in the `docker-compose.yml` or `.env` file. If you completely wipe your ClickHouse data volumes, you will need to re-run these commands on a fresh installation.
