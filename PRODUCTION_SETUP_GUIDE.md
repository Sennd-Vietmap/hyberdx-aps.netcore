# Production Observability Deployment Guide

This guide covers the necessary steps to deploy the ClickStack observability solution in a production Kubernetes environment.

---

## 1. Secrets Management
Never hardcode API Keys. Use Kubernetes Secrets to store your ClickStack/HyperDX ingestion keys.

```bash
kubectl create secret generic clickstack-secrets \
  --from-literal=api-key="YOUR_PRODUCTION_API_KEY"
```

## 2. Distributed vs. All-in-One
For production, we recommend moving away from the `all-in-one` image and using standalone ClickHouse and HyperDX components:
- **ClickHouse**: Use the [ClickHouse Operator](https://github.com/Altinity/clickhouse-operator) for scaling and clustering.
- **HyperDX**: Deploy as individual microservices (UI, API, ingestor).

## 3. Persistent Storage
Ensure your Kubernetes cluster has a storage class that supports high-performance IO (SSD/NVMe) for ClickHouse data volumes.

Refer to our [ClickHouse Clustering Guide](./CLICKHOUSE_CLUSTERING_GUIDE.md) when configuring multiple replicas.

## 4. OTel Collector Topology
In large clusters, we recommend:
1. **Host-level Agents**: DaemonSet collectors on every node to collect host metrics and logs.
2. **Gateway Collector**: A central deployment (as defined in [otel-collector.yaml](./k8s/otel-collector.yaml)) for aggregating and exporting data to ClickStack.

## 5. Network Security
- **Internal Traffic**: Ensure the OTel Collector service is only accessible within the cluster.
- **TLS**: Enable TLS for OTLP ingestion. You can configure this in the `ClickStackOptions` and provide certificates via secrets.
- **Ingress**: Use an Ingress Controller (like Nginx or Traefik) to expose the HyperDX UI securely with HTTPS.

---

## 6. Maintenance & Scaling
- **Vertical Scaling**: ClickHouse is CPU and Memory intensive. Monitor your pods and adjust `resources.limits` accordingly.
- **Horizontal Scaling**: Scale the API and Collector replicas based on load. ClickHouse should be scaled by adding shards/replicas via the Operator.
