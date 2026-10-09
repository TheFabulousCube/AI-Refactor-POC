---
name: swap-sentiment-service
description: Extracts ISentimentService, converts existing logic into a test mock, and introduces a live AISentimentService consuming the registered IChatClient. Use this skill after the AI infrastructure is registered.
version: 1.0.0
---

# Role & Core Objective

You are an expert .NET Core Refactoring Agent. Your task is to transition our string-matching sentiment flow into a decoupled, interface-based architecture that leverages an injected `IChatClient` for runtime evaluations.

# Target Architecture Constraints

1. **Abstract with Interfaces:** Extract an interface named `ISentimentService` containing the method signature `Task<string> AnalyzeSentimentAsync(string text);`. Update our Minimal API endpoint definitions to inject this interface.
2. **Isolate Testing Boundaries:** Move your old, custom string-matching and word-parsing logic into a separate file named `MockSentimentAnalysisService.cs` implementing `ISentimentService`. Keep this file completely intact for our test runner.
3. **Encapsulate Service Swapping:** Update `ServiceExtensions.cs` to map `ISentimentService` to your new live implementation. Do not pollute `Program.cs`.

# Execution Steps

### Step 1: Implement the Live AI Service Layer

Create a new file named `AISentimentService.cs` that implements the new `ISentimentService` interface.

- Inject the abstraction `IChatClient` directly into the constructor.
- Implement `AnalyzeSentimentAsync(string text)` using a defensive, strict system prompt instruction: "You are an accurate text evaluation API. Analyze the sentiment of the text. Respond with exactly one word from these options: Positive, Negative, Neutral. Do not write explanations, sentences, or extra punctuation."
- Call `chatClient.CompleteAsync(...)` asynchronously and return the trimmed response.

### Step 2: Reconfigure the Dependency Injection Map

Open `ServiceExtensions.cs`.

- Register the live runtime service mapping: `services.AddScoped<ISentimentService, AISentimentService>();`
