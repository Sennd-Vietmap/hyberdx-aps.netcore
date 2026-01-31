# ClickHouse Clustering & High Availability Guide

This guide provides a step-by-step walkthrough for setting up a ClickHouse cluster with sharding and replication.

---

## 1. Prerequisites: ClickHouse Keeper (or ZooKeeper)
ClickHouse requires a coordination service for replication and distributed DDL execution. **ClickHouse Keeper** is the modern, built-in alternative to ZooKeeper.

### Configuration (`keeper_config.xml`):
Nodes in the Keeper cluster must be defined:
```xml
<keeper_server>
    <tcp_port>2181</tcp_port>
    <server_id>1</server_id>
    <raft_configuration>
        <server>
            <id>1</id>
            <hostname>keeper-1</hostname>
            <port>9444</port>
        </server>
        <!-- Add more servers for HA -->
    </raft_configuration>
</keeper_server>
```

---

## 2. Cluster Definition (`remote_servers.xml`)
Define your shards and replicas in the ClickHouse configuration (usually under `/etc/clickhouse-server/config.d/`).

```xml
<remote_servers>
    <my_cluster>
        <shard>
            <replica>
                <host>chi-1-1</host>
                <port>9000</port>
            </replica>
            <replica>
                <host>chi-1-2</host>
                <port>9000</port>
            </replica>
        </shard>
        <shard>
            <replica>
                <host>chi-2-1</host>
                <port>9000</port>
            </replica>
        </shard>
    </my_cluster>
</remote_servers>
```

---

## 3. Macros for Portability
Macros allow you to use the same configuration file across different nodes by referencing variables.

```xml
<macros>
    <shard>01</shard>
    <replica>chi-1-1</replica>
    <cluster>my_cluster</cluster>
</macros>
```

---

## 4. Creating Replicated Tables
Use the `ReplicatedMergeTree` engine to ensure data is synchronized across replicas within a shard.

```sql
CREATE TABLE my_table ON CLUSTER '{cluster}'
(
    id UInt64,
    event_time DateTime,
    data String
)
ENGINE = ReplicatedMergeTree('/clickhouse/tables/{shard}/my_table', '{replica}')
PARTITION BY toYYYYMM(event_time)
ORDER BY (id, event_time);
```

---

## 5. Creating Distributed Tables
A Distributed table doesn't store data itself; it acts as a "view" that routes queries to the underlying shards in the cluster.

```sql
CREATE TABLE my_table_distributed ON CLUSTER '{cluster}'
AS my_table
ENGINE = Distributed('{cluster}', currentDatabase(), my_table, rand());
```

---

## 6. Verification Steps
1.  **Check Cluster Status**:
    ```sql
    SELECT * FROM system.clusters WHERE cluster = 'my_cluster';
    ```
2.  **Verify Replication**:
    ```sql
    SELECT table, is_leader, total_replicas, active_replicas 
    FROM system.replicated_tables;
    ```
3.  **Test Data Distribution**:
    Insert into the Distributed table and verify data appears on individual shard nodes.

---

## Summary of Key Concepts
| Component | Purpose |
| :--- | :--- |
| **Shard** | A subset of the whole data (Horizontal Scaling). |
| **Replica** | A copy of a shard's data (High Availability). |
| **Keeper** | Orchestrates metadata and replication logs. |
| **Distributed Engine** | Provides a single entry point for multi-shard queries. |
