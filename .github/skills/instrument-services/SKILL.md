---
name: instrument-services
description: Instruments internal .NET core services with custom OpenTelemetry child spans. Use this skill when adding granular method-level observability.
version: 1.0.0
---

# Role & Core Objective

You are an expert .NET Core Solutions Architect and technical documentation assistant. Your task is to modify the internal core services of our application to introduce method-level OpenTelemetry (OTel) `ActivitySource` tracking. This allows us to inspect execution times inside our custom processing services independently of network overhead.

# Target Architecture Constraints

1. **Preserve Extension Methods Structure:** Do NOT scatter new tracer configuration logic inside `Program.cs`. Any additions to our telemetry registration must be appended to the existing `AddCustomTelemetry` extension method in `ServiceExtensions.cs`.
2. **Do Not Touch API Endpoints:** Keep the top-level Minimal API endpoints (`/sentiment` and `/analyze`) completely intact. This experiment focuses strictly on the service implementations executing underneath them.

# Execution Steps

### Step 1: Update Telemetry Source Registration

Open `ServiceExtensions.cs`. Locate the `AddCustomTelemetry` extension method.

- Declare a public const string for our custom application service tracer: `public const string ServiceSourceName = "Custom.Application.Services";`
- Inside the `.WithTracing()` builder block, chain a new `.AddSource(ServiceSourceName)` directive. This tells the core OpenTelemetry pipeline to listen for the spans we are about to emit inside our services.

### Step 2: Instrument the Sentiment Service

Open your text sentiment service (e.g., `SentimentAnalysisService.cs`).

- Declare a private static readonly `ActivitySource` instance initialized using the exact name string from our `ServiceSourceName` ("Custom.Application.Services").
- Locate the main processing method (e.g., `AnalyzeSentiment(string text)`).
- Wrap the method's core validation and parsing logic inside an implicit `using var activity = ...` block utilizing `.StartActivity("AnalyzeSentimentExecution")`.
- Tag the activity with a custom attribute tracking the payload parameter size: `activity?.SetAttribute("custom.text.length", text?.Length ?? 0);`

### Step 3: Instrument the Analysis Service

Open `TextAnalysisService.cs` service class responsible for handling the `/analyze` endpoint requests.

- Declare a matching private static readonly `ActivitySource` instance mapped to "Custom.Application.Services".
- Locate the main analysis/counting execution method.
- Wrap its inner computation code block with `using var activity = ...` using `.StartActivity("AnalyzeWordsExecution")`.
