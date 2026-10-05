---
name: aspnet-minimal-api
description: "Create ASP.NET Minimal API endpoints with proper OpenAPI documentation"
---

# ASP.NET Minimal API with OpenAPI

Your goal is to help me create well-structured ASP.NET Minimal API endpoints with correct types and comprehensive OpenAPI/Swagger documentation.

## API Organization

- Group related endpoints using `MapGroup()` extension
- Use endpoint filters for cross-cutting concerns
- Structure larger APIs with separate endpoint classes
- Consider using a feature-based folder structure for complex APIs

## Request and Response Types

- Define explicit request and response DTOs/models
- Create clear model classes with proper validation attributes
- Use record types for immutable request/response objects
- Use meaningful property names that align with API design standards

## Error Handling and Validation

- Use `Results.BadRequest()` for validation errors
- Return `Results.NotFound()` for missing resources
- Do NOT gracefully handle exceptions in the endpoint; let them bubble up to middleware for logging and standardized error responses

## Type Handling

- Use strongly-typed route parameters with explicit type binding
- Use `Results<T1, T2>` to represent multiple response types
- Return `TypedResults` instead of `Results` for strongly-typed responses
- Leverage C# 10+ features like nullable annotations and init-only properties
