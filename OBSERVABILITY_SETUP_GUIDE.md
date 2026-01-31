# Understanding the Observability Stack (A Beginner's Guide)

This guide breaks down how our Docker infrastructure and OpenTelemetry (OTel) Collector work together to monitor your application.

---

## 1. What is `docker-compose.yml`?
Think of **Docker Compose** as a "Recipe" for your entire system. Instead of starting 5 different programs manually, Docker Compose starts them all at once with the right settings.

### Key Parts Explained:

#### A. Services (The Workers)
Each `service` is like a mini-server (container) running one specific task:
- **`clickstack-observability` (ClickStack)**: This is the "Brain." It collects all the data (logs, traces, metrics) and shows it to you in a nice dashboard. It's powered by the official `clickhouse/clickstack-all-in-one` image.
- **`nginx-proxy`**: This is the "Front Door." It takes requests from your browser and sends them to the ClickStack UI.
- **`otel-collector`**: This is the "Post Office." It receives data from your API and Nginx, organizes it, and sends it to the Brain (ClickStack).

#### B. Ports (The Windows)
- `"8080:8080"`: Maps your computer's port (left) to the container's port (right). This is how you access the UI at `localhost:8080`.

#### C. Volumes (The Shared Folders)
- `nginx_logs`: A shared folder between Nginx and the OTel Collector. Nginx writes logs there, and the Collector reads them.

---

## 2. What is `otel-collector-config.yaml`?
This file tells the **OpenTelemetry Collector** exactly what to do with the data it receives. It follows a simple flow: **Receive -> Process -> Export**.

### Key Concepts:

#### A. Receivers (How we get data)
- **`otlp`**: Listens for data sent from your ASP.NET API (using the OTLP protocol).
- **`filelog`**: "Tails" (watches) the Nginx log file for new entries.

#### B. Processors (How we change data)
- **`batch`**: Waits for a small amount of data to pile up before sending it. This is more efficient than sending every single log entry one by one.
- **`transform`**: Adds "Labels" to the data. For example, it adds `service.name: nginx-proxy` so you know where the logs came from.

#### C. Exporters (Where we send data)
- **`otlphttp`**: Sends everything we've collected and modified to the HyperDX backend (`clickstack-hyperdx:4318`).

#### D. Service Pipelines (The Routes)
The Pipelines connect the parts together:
- `logs`: Receives OTLP + File logs -> Batches/Transforms them -> Exports to HyperDX.
- `metrics`: Receives OTLP metrics -> Batches them -> Exports to HyperDX.
- `traces`: Receives OTLP traces -> Batches them -> Exports to HyperDX.

---

## Summary Flow
1. **Your API** sends a "Trace" (a record of a request) to the **Collector**.
2. **Nginx** writes a "Log" entry to a shared file.
3. The **Collector** picks up both.
4. The **Collector** adds extra info (like service names).
5. The **Collector** sends everything to **HyperDX**.
6. **You** see everything on the dashboard at `http://localhost:8686`.
