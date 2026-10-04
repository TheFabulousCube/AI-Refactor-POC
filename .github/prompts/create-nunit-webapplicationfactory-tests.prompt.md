---
name: create-nunit-webapplicationfactory-tests
description: Create an NUnit WebApplicationFactory integration test suite for an ASP.NET Core Minimal API.
---

# Create NUnit WebApplicationFactory Test Suite

Follow the skill instructions in:

#files:"skills/nunit-webapplicationfactory/SKILL.md"

Use the templates in:

#files:"skills/nunit-webapplicationfactory/templates/CustomWebApplicationFactory.cs.tpl"
#files:"skills/nunit-webapplicationfactory/templates/ApiTestFixture.cs.tpl"
#files:"skills/nunit-webapplicationfactory/templates/HealthTests.cs.tpl"
#files:"skills/nunit-webapplicationfactory/templates/TestAuthHandler.cs.tpl"
#files:"skills/nunit-webapplicationfactory/templates/WebApplicationFactoryExtensions.cs.tpl"

## Task

Create or update the NUnit WebApplicationFactory integration test suite for this solution.

## Requirements

- Use NUnit.
- Use `Microsoft.AspNetCore.Mvc.Testing`.
- Use `WebApplicationFactory<Program>`.
- Use `HttpClient`.
- Do not use FluentAssertions.
- Use NUnit assertions only.
- Do not call production databases.
- Do not call real external services.
- Set the ASP.NET Core environment to `Testing`.
- Add `public partial class Program` to the API project's `Program.cs` if needed.
- Create a smoke test for the health endpoint if available.

## Inputs

If not obvious from the solution, ask me for:

- API project path
- Test project path
- Root namespaces
- Health endpoint path
- Whether the API uses EF Core
- Whether authenticated endpoints need test coverage

## Expected output

Create or update:

- `{{TestProjectName}}/{{TestProjectName}}.csproj`
- `{{TestProjectName}}/Infrastructure/CustomWebApplicationFactory.cs`
- `{{TestProjectName}}/Infrastructure/ApiTestFixture.cs`
- `{{TestProjectName}}/Endpoints/HealthTests.cs`

Only add auth and database helper files if needed.

## Verification

After making changes, run or recommend:

```bash
dotnet test