---
name: nunit-webapplicationfactory-minimal-api-tests
description: Create and maintain an NUnit integration test suite for a C# ASP.NET Core Minimal API using WebApplicationFactory.
---

# NUnit WebApplicationFactory Minimal API Test Suite

Use this skill to create or extend an integration test suite for a C# ASP.NET Core Minimal API using:

- NUnit
- Microsoft.AspNetCore.Mvc.Testing
- WebApplicationFactory<TProgram>
- HttpClient
- NUnit assertions

Do not use FluentAssertions unless the user explicitly asks for it.

The goal is to test API behavior through real HTTP requests against an in-memory ASP.NET Core test host.

---

## Testing Approach

Use `WebApplicationFactory<Program>` for API-level integration tests.

This is appropriate for testing:

- Minimal API endpoint routing
- Request and response serialization
- Model binding
- Validation behavior
- Middleware behavior
- Dependency injection configuration
- Authentication and authorization behavior
- Database integration using a test database or in-memory provider
- Error responses
- Status codes and response payloads

Do not use this skill for browser UI testing. Use Playwright for browser-driven end-to-end tests.

---

## Expected Project Structure

Assume a solution similar to:

```text
MyApp.sln
src/
  MyApp.Api/
    MyApp.Api.csproj
    Program.cs
tests/
  MyApp.Api.Tests/
    MyApp.Api.Tests.csproj
```

If the test project does not exist, create it under `tests/` or follow the pattern of the project under test.

---

## Required Packages

Add these packages to the NUnit test project:

```bash
dotnet add tests/{{TestProjectName}} package NUnit
dotnet add tests/{{TestProjectName}} package NUnit3TestAdapter
dotnet add tests/{{TestProjectName}} package Microsoft.NET.Test.Sdk
dotnet add tests/{{TestProjectName}} package Microsoft.AspNetCore.Mvc.Testing
```

If the API uses EF Core and the user wants an in-memory database:

```bash
dotnet add tests/{{TestProjectName}} package Microsoft.EntityFrameworkCore.InMemory
```

Add a project reference from the test project to the API project:

```bash
dotnet add tests/{{TestProjectName}} reference src/{{ApiProjectName}}/{{ApiProjectName}}.csproj
```

---

## Template Variables

When using files from `templates/`, replace these placeholders:

| Placeholder | Meaning | Example |
|---|---|---|
| `{{ApiProjectName}}` | API project name | `MyApp.Api` |
| `{{TestProjectName}}` | Test project name | `MyApp.Api.Tests` |
| `{{ApiRootNamespace}}` | API root namespace | `MyApp.Api` |
| `{{TestRootNamespace}}` | Test root namespace | `MyApp.Api.Tests` |
| `{{DbContextName}}` | EF Core DbContext name, if applicable | `AppDbContext` |
| `{{HealthEndpoint}}` | Health check path | `/health` |

If a value is unknown, infer it from the project files. If it cannot be inferred safely, ask the user.

---

## Minimal API Program Requirement

For Minimal API projects using top-level statements, make sure `Program.cs` exposes a public partial `Program` class.

At the bottom of `src/{{ApiProjectName}}/Program.cs`, add:

```csharp
public partial class Program
{
}
```

Do not add this if it already exists.

This allows the test project to use:

```csharp
WebApplicationFactory<Program>
```

---

## Test Project Layout

Create this structure:

```text
tests/
  {{TestProjectName}}/
    Infrastructure/
      CustomWebApplicationFactory.cs
      ApiTestFixture.cs
    Endpoints/
      HealthTests.cs
```

Optional files:

```text
tests/
  {{TestProjectName}}/
    Infrastructure/
      TestAuthHandler.cs
      WebApplicationFactoryExtensions.cs
```

Use the files from the `templates/` directory as the source for these files.

---

## Core Files to Generate

Generate these files by copying and adapting templates:

1. `templates/CustomWebApplicationFactory.cs.tpl`
   - Output to:
     `tests/{{TestProjectName}}/Infrastructure/CustomWebApplicationFactory.cs`

2. `templates/ApiTestFixture.cs.tpl`
   - Output to:
     `tests/{{TestProjectName}}/Infrastructure/ApiTestFixture.cs`

3. `templates/HealthTests.cs.tpl`
   - Output to:
     `tests/{{TestProjectName}}/Endpoints/HealthTests.cs`

Only generate auth/database helper templates if the project needs them.

---

## Custom WebApplicationFactory Rules

The generated `CustomWebApplicationFactory` should:

- Inherit from `WebApplicationFactory<Program>`
- Set the environment to `Testing`
- Override app configuration only when needed
- Replace production-only services with test doubles
- Avoid real production databases
- Avoid real external APIs
- Keep per-test state isolated where possible

When replacing services, remove the existing service descriptor before registering the test service.

---

## Base Test Fixture Rules

The generated base fixture should:

- Create a new `CustomWebApplicationFactory`
- Create an `HttpClient`
- Disable automatic redirects by default
- Dispose the client and factory after each test

Use `[SetUp]` and `[TearDown]` by default for isolation.

If startup time becomes expensive, the user may switch to `[OneTimeSetUp]` and `[OneTimeTearDown]`, but warn that this introduces shared state risks.

---

## Assertion Style

Use NUnit assertions.

Preferred style:

```csharp
Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
Assert.That(body, Is.Not.Null);
Assert.That(body!.Status, Is.EqualTo("ok"));
```

Do not use FluentAssertions.

---

## Example Endpoint Test Patterns

### GET endpoint

```csharp
var response = await Client.GetAsync("/resource");

Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
```

### POST endpoint

```csharp
var response = await Client.PostAsJsonAsync("/resource", request);

Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
```

### Deserialize JSON

```csharp
var body = await response.Content.ReadFromJsonAsync<ResponseDto>();

Assert.That(body, Is.Not.Null);
Assert.That(body!.Name, Is.EqualTo("Expected"));
```

### Assert response header

```csharp
Assert.That(response.Headers.Location, Is.Not.Null);
```

### Assert content type

```csharp
Assert.That(response.Content.Headers.ContentType?.MediaType, Is.EqualTo("application/json"));
```

### Testing with Different Data Sets
Use NUnit's [TestCase] attribute:
```csharp
[TestCase(1, HttpStatusCode.OK)]
[TestCase(999, HttpStatusCode.NotFound)]
[TestCase(-1, HttpStatusCode.BadRequest)]
public async Task GetItem_WithVariousIds_ReturnsExpectedStatusCode(
    int id, HttpStatusCode expectedStatus)
{
    var response = await Client.GetAsync($"/items/{id}");
    Assert.That(response.StatusCode, Is.EqualTo(expectedStatus));
}
```


---

## Database Testing Guidance

If the API uses EF Core, prefer one of these approaches:

1. In-memory database for simple integration tests
2. SQLite in-memory for relational behavior
3. Testcontainers for production-like database behavior

Avoid using the production database.

For EF Core in-memory testing:

- Replace `DbContextOptions<TDbContext>`
- Use a unique database name per test factory instance
- Ensure the database is created before tests run
- Seed data explicitly inside each test

Do not rely on test execution order.

---

## Test Data Seeding

If database seeding is required, generate:

```text
Infrastructure/WebApplicationFactoryExtensions.cs
```

from:

```text
templates/WebApplicationFactoryExtensions.cs.tpl
```

Use explicit seed data inside each test.

---

## Authentication Testing Guidance

If the API has authenticated endpoints, generate a fake test authentication handler from:

```text
templates/TestAuthHandler.cs.tpl
```

The test authentication handler should:

- Use a deterministic fake user
- Avoid external identity providers
- Register a test authentication scheme
- Allow tests to exercise authorization logic without real tokens

---

## Recommended Test Naming

Use behavior-focused test names:

```text
MethodName_StateUnderTest_ExpectedBehavior
```

Examples:

```csharp
GetHealth_WhenCalled_ReturnsOk
GetTodo_WhenTodoExists_ReturnsOk
GetTodo_WhenTodoDoesNotExist_ReturnsNotFound
CreateTodo_WithValidRequest_ReturnsCreated
CreateTodo_WithInvalidRequest_ReturnsBadRequest
DeleteTodo_WhenUserIsUnauthorized_ReturnsUnauthorized
```

---

## First Test to Add

Always add a simple smoke test first.

Preferred:

```text
Endpoints/HealthTests.cs
```

If the API does not have `/health`, use an existing simple GET endpoint.

If no simple endpoint exists, ask the user whether to add a health endpoint.

---

## CI Verification

After generating the suite, verify with:

```bash
dotnet test
```

Or:

```bash
dotnet test tests/{{TestProjectName}}/{{TestProjectName}}.csproj
```

---

## Checklist

Before finishing, verify:

- [ ] Test project exists
- [ ] Test project references the API project
- [ ] NUnit package is installed
- [ ] NUnit3TestAdapter package is installed
- [ ] Microsoft.NET.Test.Sdk package is installed
- [ ] Microsoft.AspNetCore.Mvc.Testing package is installed
- [ ] `Program.cs` exposes `public partial class Program`
- [ ] `CustomWebApplicationFactory` exists
- [ ] `ApiTestFixture` exists
- [ ] At least one endpoint test exists
- [ ] No FluentAssertions usage exists
- [ ] Tests use NUnit assertions
- [ ] Tests do not use production databases
- [ ] Tests do not call real external services
- [ ] Tests run with `dotnet test`

---

## Anti-Patterns to Avoid

Avoid:

- Using FluentAssertions unless explicitly requested
- Calling production databases
- Calling external APIs
- Sharing mutable database state across unrelated tests
- Depending on test execution order
- Only testing status codes when response payload matters
- Over-mocking the ASP.NET Core request pipeline
- Starting a real Kestrel server unless explicitly required
- Using Playwright for pure API tests

---
