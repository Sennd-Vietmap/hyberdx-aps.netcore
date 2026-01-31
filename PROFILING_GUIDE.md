# .NET Continuous Profiling Guide

Continuous Profiling allows you to see exactly which lines of code are consuming CPU or Memory, even in production, with minimal overhead.

---

## 1. Why Profiling?
- **Traces** tell you *which* service or method is slow.
- **Profiling** tells you *why* (e.g., high CPU usage in a specific LINQ query or memory pressure from large object allocations).

## 2. Enabling Profiling in .NET
OpenTelemetry .NET is currently working on native profiling support. For now, the most common way to integrate profiling is using the **OpenTelemetry .NET Bootstrapper** or specific vendor agents.

### A. Add the Profiling Package
```powershell
dotnet add package OpenTelemetry.Instrumentation.Runtime
```
*(We have already included this in our `ClickHouse.ClickStack.AspNetCore` library)*.

### B. What it tracks:
- **CPU Usage**: Identify methods that peg the CPU.
- **Allocation Rate**: Track how many objects your app is creating.
- **GC Duration**: See how much time your app spends in Garbage Collection.

## 3. Visualizing in ClickStack (HyperDX)
HyperDX integrates profiling data directly into the UI.
1. Go to the **Profiling** tab.
2. Select your `clickstack-demo-api` service.
3. Look at the **Flame Graph** to see the call stack with the highest execution time.

## 4. Correlating Traces and Profiles
One of the most powerful features of ClickStack is **Span-Profile Correlation**. When you look at a slow trace, you can click on a span and jump directly to the profile of the thread that was executing that span at that exact moment.

---

> [!NOTE]
> Profiling is still evolving in the OTel ecosystem. Keep an eye on the [OTel .NET Profiling](https://github.com/open-telemetry/opentelemetry-dotnet) repository for the latest stable implementations.
