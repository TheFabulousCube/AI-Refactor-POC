# Instrumenting Observability for TextAnalyzer.Api

This plan outlines the steps to add OpenTelemetry instrumentation to the `TextAnalyzer.Api` project.

## Goals
- Add distributed tracing and metrics collection.
- Export data via OTLP to `http://localhost:4317`.
- Maintain clean separation of concerns by using `ServiceExtensions.cs`.

## Tasks

### 1. Project Preparation
- [x] Create `docs/plan009.md`.

### 2. Dependency Installation
- [x] Install `OpenTelemetry.Extensions.Hosting` in `src/TextAnalyzer.Api`.
- [x] Install `OpenTelemetry.Instrumentation.AspNetCore` in `src/TextAnalyzer.Api`.
- [x] Install `OpenTelemetry.Instrumentation.Http` in `src/TextAnalyzer.Api`.
- [x] Install `OpenTelemetry.Exporter.OpenTelemetryProtocol` in `src/TextAnalyzer.Api`.

### 3. Implementation
- [x] Implement `AddCustomTelemetry` in `src/TextAnalyzer.Api/ServiceExtensions.cs`.
	- Set Resource Service Name to "SentimentAnalysisBaselineApi".
	- Configure Tracing with AspNetCore and Http instrumentation.
	- Configure Metrics with AspNetCore and Http instrumentation.
	- Configure OTLP exporter for both.
- [x] Register `AddCustomTelemetry` in `src/TextAnalyzer.Api/Program.cs`.

### 4. Verification
- [x] Build the solution.
- [x] Run the tests.
- [x] Update the README.md.
