# Frontend RUM (Real User Monitoring) Guide

This guide explains how to connect your Frontend (React, Next.js, or Vue) to your ClickStack observability loop.

---

## 1. Why RUM?
RUM allows you to:
- See the **End-to-End Trace**, starting from the user's browser all the way to your ClickHouse database.
- Capture **Session Replays** to see exactly what the user saw during an error.
- Measure **Core Web Vitals** (LCP, FID, CLS).

## 2. Integration with HyperDX Browser SDK

### A. Install the SDK
```bash
npm install @hyperdx/browser
```

### B. Initialize in your App (e.g., `App.js` or `_app.tsx`)
```javascript
import HyperDX from '@hyperdx/browser';

HyperDX.init({
  apiKey: 'YOUR_HYPERDX_API_KEY',
  service: 'my-frontend-app',
  tracePropagationTargets: [/localhost/, /your-api-domain/], // Enable parent-child trace correlation
  consoleCapture: true,  // Capture console.logs
  sessionReplay: true,   // Enable session replay
});
```

## 3. Correlating with Backend Traces
The `tracePropagationTargets` setting is crucial. It tells the Browser SDK to attach trace headers when making requests to your API. 

When your API (using `ClickHouse.ClickStack.AspNetCore`) receives these headers, it will automatically link the browser trace with the server-side trace.

## 4. Viewing in ClickStack
- **Session Replay**: Go to the **Sessions** tab in HyperDX to watch recordings of user interactions.
- **Frontend Errors**: Browser console errors appear in the **Logs** tab with full stack traces.
- **Trace Waterfall**: Click on a trace to see the browser click -> network request -> API processing -> DB query flow.

---

> [!TIP]
> Use the `app.account_id` tag on the frontend side too:
> ```javascript
> HyperDX.setGlobalAttributes({
>   'app.account_id': currentUser.id,
> });
> ```
> This ensures consistent filtering across your entire stack.
