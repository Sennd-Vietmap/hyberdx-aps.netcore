# Finding HyperDX Dashboard Templates

HyperDX is a modern, open-source platform. While it doesn't have a single "App Store" for dashboards yet, there are several ways to get pre-built templates for free.

---

## 1. Built-in Templates (Coming Soon/Native)
The HyperDX team is continuously adding "Auto-Dashboards." When you connect a known data source (like Nginx, Redis, or PostgreSQL via OpenTelemetry), HyperDX often detects the attributes and suggests or automatically builds a dashboard for you.

## 2. Official HyperDX GitHub & Documentation
The best source for official JSON templates is the HyperDX GitHub repository and their documentation site.
- **Official Docs**: [hyperdx.io/docs](https://www.hyperdx.io/docs)
- **GitHub Repository**: [github.com/HyperDX/hyperdx](https://github.com/HyperDX/hyperdx)
  - Look for a `dashboards/` or `examples/` folder in the source code.

## 3. Community Sources
Since HyperDX uses a standard JSON format for dashboards, you can find templates shared by the community in:
- **HyperDX Discord**: The community often shares JSON snippets in the #dashboards or #showcase channels.
- **GitHub Search**: Search for `HyperDX Dashboard JSON` to find Gists or repositories from other developers.

---

## 4. How to Import a Template
Once you have a dashboard JSON file or snippet:
1.  Open your HyperDX UI (`http://localhost:8686`).
2.  Go to the **Dashboards** tab.
3.  Look for the **Import** button (usually near "+ New Dashboard").
4.  Paste the JSON code or upload the file.

## 5. Share Your Own
If you build a great dashboard for your .NET API or Nginx setup, you can export it:
1.  Open your dashboard.
2.  Click the **...** (Options) menu.
3.  Select **Export as JSON**.
4.  You can now save this file and share it with your team or the community!

---

> [!TIP]
> Because HyperDX is compatible with OpenTelemetry, any guide for "OpenTelemetry Dashboard for [Service X]" will give you the right **Metric Names** and **Attributes** to use, even if you have to build the chart manually in the HyperDX UI.
