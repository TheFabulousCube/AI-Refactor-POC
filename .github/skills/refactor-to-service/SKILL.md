---
name: refactor-to-service
description: Extracts inline endpoint execution logic into decoupled C# Service classes
---

# Instructions

1. Analyze the targeted API endpoint method block and identify business logic, validation rules, mapping, and any long-running or reusable processing.
2. Create a new service interface (for example, `ITextService`) with clear asynchronous method signatures that represent the business operation rather than HTTP concerns.
3. Implement a corresponding service class using modern C# 12+ primary constructors and keep the service focused on domain/business logic only.
4. Move validation, mapping, and heavy processing logic into the new service, but leave request parsing, model binding, and HTTP status/result generation in the endpoint.
5. Register the service in dependency injection in `Program.cs` or the app startup configuration using the appropriate lifetime (`AddScoped`, `AddSingleton`, or `AddTransient`).
6. Update the Minimal API route map or Controller action to accept the interface via dependency injection and keep the endpoint handler ultra-thin.
7. Return clean `Results` or HTTP status wrappers from the endpoint while the service returns domain data or operation results.
8. Create or update tests for the extracted behavior and endpoint wiring, and reference the relevant guidance in #file:.github/skills/csharp-xunit/SKILL.md when writing or updating the tests.
9. Validate the refactor by running the relevant `.NET` test command and checking that the extracted logic still behaves correctly.

## Guardrails

- Do not move `Results.Ok(...)`, `Results.NotFound(...)`, `BadRequest`, authentication/authorization checks, or other ASP.NET Core HTTP concerns into the service.
- Prefer a thin endpoint/controller that transforms HTTP input into service calls and maps service results to HTTP responses.
- Keep service methods testable in isolation so unit tests can verify business rules without bootstrapping the whole web application.
- If the service is used by multiple endpoints, prefer shared business logic in the service rather than duplicating the same code in each route.
