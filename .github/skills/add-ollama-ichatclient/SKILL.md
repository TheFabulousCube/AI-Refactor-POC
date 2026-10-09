---
name: add-ollama-ichatclient
description: Installs OllamaSharp and registers a telemetry-wrapped Microsoft.Extensions.AI IChatClient pointing to a local model instance.
version: 1.1.0
inputs:
  model_name:
    description: "The name and size tag of the local Ollama model to target"
    default: "llama3.2"
---

# Role & Core Objective

You are an expert .NET Core Infrastructure Engineer. Your task is to inject a standard local `IChatClient` engine into our application's service collection, fully instrumented with OpenTelemetry tracking, using the target local model specified in our inputs: `${inputs.model_name}`.

# Target Architecture Constraints

1. **Encapsulate in Extensions:** Do NOT write the initialization or builder pipeline logic inside `Program.cs`. Append all registrations to the existing `ServiceExtensions.cs` file.
2. **Do Not Touch Endpoints:** Leave the current Minimal API routing logic and service assignments completely alone. This phase is strictly infrastructure preparation.

# Execution Steps

### Step 1: Install Required Package Dependencies

Utilize the terminal workspace utility to add the following specific packages to the project:

- `OllamaSharp`
- `Microsoft.Extensions.AI`

### Step 2: Register IChatClient Middleware Pipeline

Open `ServiceExtensions.cs`. Inside your telemetry or service configuration layer, register the native `IChatClient` abstraction:

- Instantiate the core Ollama driver targeting your local instance, passing the exact model parameter identifier: `IChatClient innerClient = new OllamaApiClient(new Uri("http://localhost:11434"), "${inputs.model_name}");`
- Wrap the client inside a standard `ChatClientBuilder` pipeline and attach the native OpenTelemetry tracking layer:
  ```csharp
  services.AddSingleton<IChatClient>(sp =>
      new ChatClientBuilder(innerClient)
          .UseOpenTelemetry(sourceName: "Experimental.Microsoft.Extensions.AI")
          .Build());
  ```
