---
name: instrument-observability
description: Instruments a .NET Core API with a baseline OpenTelemetry pipeline targeting a local OTLP collector. Use this skill when asked to add observability, instrument endpoints, or configure tracing and metrics.
version: 1.0.0
---

# Role & Core Objective

You are an expert .NET Core Solutions Architect optimization agent. Your task is to modify the provided application to inject a baseline OpenTelemetry (OTel) metrics and distributed tracing pipeline routed to a local OTel collector container.

# Target Architecture Constraints

1. **No Direct Program.cs Pollution:** Do NOT write the configuration middleware block directly inside `Program.cs`. All `IServiceCollection` configurations must be cleanly encapsulated inside our project's existing `ServiceExtensions.cs` file.
2. **Preserve Processing Logic:** Do NOT break or change the internal processing code of our existing Minimal API POST endpoints: `/sentiment` and `/analyze`. Keep their internal validation and data return structures exactly as they are currently written.
3. **Strict Domain Scoping:** Do not suggest, introduce, or install any external AI, LLM, or semantic kernel packages during this phase. Maintain a lightweight framework footprint optimized for local verification.

# Execution Steps

### Step 1: Execute Package Installations

Directly utilize the workspace package manager to install the following specific OpenTelemetry dependencies into the project:

- OpenTelemetry.Extensions.Hosting
- OpenTelemetry.Instrumentation.AspNetCore
- OpenTelemetry.Instrumentation.Http
- OpenTelemetry.Exporter.OpenTelemetryProtocol

### Step 2: Implement the Extension Method

Open `ServiceExtensions.cs`. Append or implement a public static extension method signature named `AddCustomTelemetry(this IServiceCollection services)`.

- Establish the baseline application target Resource Service Name as "SentimentAnalysisBaselineApi".
- Configure Distributed Tracing via `.WithTracing()` and chain both `AddAspNetCoreInstrumentation()` and `AddHttpClientInstrumentation()`.
- Configure Metrics collection via `.WithMetrics()` and chain both `AddAspNetCoreInstrumentation()` and `AddHttpClientInstrumentation()`.
- Configure the OTLP Exporter (`.AddOtlpExporter()`) for both pipelines to point directly to the local container endpoint at: `http://localhost:4317`.

### Step 3: Register in Application Pipeline

Open `Program.cs`. Locate the container service registration area.

- Inject a clean call to `builder.Services.AddCustomTelemetry();`.
- Verify that the target POST endpoints `/sentiment` and `/analyze` remain untouched and operational.
