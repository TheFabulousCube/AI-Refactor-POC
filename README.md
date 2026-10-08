# Copilot Instructions, Skills, and Agents POC

> A continuous log and framework for forcing AI code generation tools to refactor legacy tutorial code under strict workspace constraints—without writing a single line of manual code.

## The Core Philosophy & Constraint

- **Zero Manual Coding:** I am not creating folders, changing variables, or writing code. Everything outside of the .github folder is handled exclusively by the AI.
- **Prompt-Driven Engineering:** If the AI fails, I do not fix the code. I roll back the changes in Git, refine my prompts, custom instructions, or skills, and run it again.
- **The Ultimate Goal:** To iterate until the workflow is flawless, resulting in a reusable, plug-and-play set of instructions, skills, and agents that can automatically refactor tightly coupled tutorial code across any .NET project.

---

## The Workspace Strategy

### 1. Custom Instructions (.github/copilot-instructions.md)

Custom instructions enforce coding standards, language patterns, or architectural rules across the entire solution.

- **Setup Process:**
  1. Created .github/copilot-instructions.md in the root folder.
  2. Defined explicit behavior rules using Markdown.
  3. Enabled the feature in Visual Studio via: Tools > Options > GitHub > Copilot > Copilot Chat -> Check "Enable custom instructions to be loaded from .github/copilot-instructions.md files".
- **Status:** Currently Experimenting. I injected an explicit rule to preface all endpoints with tfc- as a visual test to verify if the instructions are being loaded and actively respected by the local model.

### 2. Copilot Skills (.github/skills/\*/SKILL.md)

Skills are targetable behavior routines that can be explicitly invoked in the chat window.

- **LLM Context Constraints:**
  - Failed: qwen3:8b failed to pick up custom skills (likely due to a small context window).
  - Passed: qwen3-coder:30b successfully parses and executes custom skills.
- **Early Architectural Lesson:** When tasked with refactoring endpoints into a dedicated Services folder, the model successfully implemented Dependency Injection (DI) but jammed the configuration directly into Program.cs and duplicated models. Lesson learned: Rules must explicitly restrict model duplication and define clean DI registration habits.

---

## Tooling & Workflow Log

### Experiment 009: Add Unit Tests for Services - gemma4:26b

> Adding instructions to run a build and run the tests before and after code changes certaily cut down on my involvment,  
> gemma forgot the using statements and miscounted some characters in the test, but figured it out without my help.  
> I don't see any explicit SKILL calls in this round, but the work really was pretty simple.

**Goals:**

- Create unit tests for `TextAnalysisService` and `SentimentAnalysisService` to ensure business logic correctness.
- Organize tests into a dedicated `Services` subfolder within the test project.

**What Worked:**

1. Created `tests/TextAnalyzer.Api.Tests/Services/` directory.
2. Implemented `TextAnalysisServiceTests.cs` covering null, empty, and various text inputs.
3. Implemented `SentimentAnalysisServiceTests.cs` covering sentiment variations.
4. Verified all 29 tests pass successfully.

**Key Learnings:**

- Naive sentiment analysis (string splitting without punctuation handling) requires careful test data to avoid false positives/negatives.
- Character count assertions must account for all whitespace characters (e.g., `\t`, `\n`, `\r`).


### Experiment 008: Refactor TextAnalyzer.Api to use Services - gemma4:26b
> I think I've found a sweet spot with gemma4:26b!  Large enough to do work, but not so slow!  
> The model created a plan.md, so I figured I'd include it in a 'docs' folder  
> It skipped the 'update-readme' skill the first time, I rolled that back and had it do it over

**Goals:**

- Refactor the business logic in `Program.cs` into separate services and use Dependency Injection.

**What Worked:**

1. Defined `ITextAnalysisService` and `SentimentAnalysisService` interfaces.
2. Created `TextAnalysisService` and `SentimentAnalysisService` implementations in `src/TextAnalyzer.Api/Services/`.
3. Created `ServiceExtensions.cs` and implemented `AddTextAnalyzerServices`.
4. Registered services in `Program.cs`.
5. Updated `Program.cs` endpoints to use the new services.
6. Verified everything by running build and tests.

**Key Learnings:**

- Moving business logic to services and using DI improves testability and maintainability.
- Ensuring all interfaces and implementations are correctly registered in the DI container is crucial.

### Experiment 007 - 2026-10-07 - TFC-Copilot
> qwen3:8b got lost in the folder structure, I had to cancel that and start over.  
> qwen3-coder handled it just fine, but it spills over from my GPU to the CPU and is very slow.  
> qwen3-coder also made a mess of this template, but that could be partly my fault.

**Model:** qwen3-coder-next:latest

**Goals:**
- Refactor models from Program.cs into a separate Models/ folder
- Maintain all existing functionality
- Ensure tests continue to pass

**What Worked:**
- Created Models/ folder with 4 separate model files
- Updated Program.cs to remove model definitions
- All 15 tests pass after refactoring
- Project builds successfully

**Key Learnings:**
- Ensured proper namespace usage
- Maintained C# 12+ syntax with primary constructors
- All tests passed without modification

### Experiment 006: Test Validation & Endpoint Standardization - qwen3:8b

> With solid, passing tests in place, I'm finally free to start refactoring! explicitly
> I updated the aspnet-minimal-api skill to explicitly handle error conditions since that was missed. Some of the models do it on their own, some have to be told. Since they're all indeterminate by nature, I think it's best to explicitly state things I know I want.  
> I'm not really happy with SKILLS that are downloaded or written by AI (or both).

**Goals:**

- Fix invalid test logic in two integration tests
- Ensure endpoints follow proper error handling patterns
- Verify all tests pass against updated implementations

**What Worked:**

1. Fixed `AnalyzeTextEndpoint_HandlesWhitespaceOnlyString`: Corrected assertion to expect 7 character count (not 4) for whitespace-only input
2. Fixed `AnalyzeSentimentEndpoint_HandlesEdgeCases`: Updated expectations to match actual sentiment analysis logic (equal positive/negative → Neutral, no sentiment words → Neutral)
3. Updated both `/tfc-analyze` and `/tfc-sentiment` endpoints to use consistent error handling patterns and response structures
4. All 8 integration tests now pass successfully
5. Verified endpoints return proper HTTP status codes (200 OK for valid requests)

**Key Learnings:**

- Test assertions must accurately reflect actual implementation behavior
- Whitespace handling and sentiment edge cases require precise validation
- Consistent endpoint patterns across the API improve maintainability

**Next Steps:**

- Continue refining skills for automatic code refactoring
- Explore NUnit integration test generation patterns
- Investigate performance optimization for text analysis operations

### Experiment 005: xUnit Test Fix & Skill Enhancement

> Yay! The models have learned to update the README on their own!
> I broke it up, qwen3-coder:30b did the work, and qwen3:8b did the documentation.

**Goals:**

- Resolve xUnit test failures caused by missing references
- Enhance code audit skills for better compliance checking

**What Worked:**

1. Added `using Xunit;` to test file resolving CS0246 errors
2. Installed `xunit.analyzers` for code analysis
3. Successfully ran all tests after fixing package references
4. Enhanced the `net-audit` skill to include more detailed compliance checks

**Next Steps:**

- Continue refining skills for automatic code refactoring
- Document additional test cases for edge scenarios
- Explore further automation of API endpoint standardization

### Experiment 004: Starting over

Goals:

- Start over with a fresh repo using separate folders for better structure
- Use local Ollama models to rebuild the project, including tests from scratch

What worked:
Breaking the procedure into smaller steps, at least it helped me

1. I deleted everything and added back this README.md and the .github folder
2. I had the AI build things back one step at a time.

### Experiment 003: Creating Test Project using custom Prompt/SKILL/templates with Kimi K3

Goals:

1. I wanted a solid test suite to make sure things didn't break in the future
2. I wanted to try out templates for the files generated in a SKILL
   I had AI generate the skill and the templates. It didn't work out _exactly_ as I wanted, but I wanted to move fast and figured I'd redo this step a few times.

I started with my local `Qwen3-coder` model, but it took forever and I eventually stopped it.

Kimi really did a great job, but I realized my initial folder structure was naive.

> I'm documenting this step, but starting over for the next step

### Experiment 002: Ollama + BYOM (Free/Local)

Using qwen3-coder:30b, I created a targeted skill file to test adherence to strict standards:

- **File Created:** .github/skills/audit/net-audit.md

  ```markdown
  ---
  name: net-audit
  description: Audits code against internal C# and API prefix routing standards
  ---

  # Instructions

  1. Inspect all API route templates in the target file.
  2. If any endpoint string lacks the "tfc-" prefix, fail the audit and rewrite the code to include it.
  3. Verify C# 12 primary constructor usage and ensure public methods have XML docs.
  ```

#### Test Case A: Auditing Existing Code

Running the audit against the initial Program.cs file yielded perfect compliance logs. The model identified missing route prefixes and missing XML documentation, then accurately refactored the file:

- Changed /analyze to /tfc-analyze
- Injected structured XML comments
- Preserved C# 12 primary constructor patterns on records

#### Test Case B: Generating New Features via Skills

- **Prompt Provided:**
  ```text
  /net-audit Create a new HTTP POST endpoint called "/sentiment" that accepts a text payload. It must evaluate the string and return a basic sentiment score (Positive, Negative, Neutral). Ensure the code strictly adheres to all our .github workspace rules.
  ```
- **Result:** Success. It generated a brand new /tfc-sentiment endpoint that natively adopted the tfc- prefix rule from the skill architecture and correctly guessed a basic scoring payload structure.

### Experiment 001: Claude Code (Paid/Cloud)

- **Prompt Provided:**
  ```text
  Create a new ASP.NET Core Minimal API project in this folder. Add one endpoint, a POST to /analyze, that accepts a JSON body with a "text" field, and returns the word count and character count of that text as JSON.
  ```
- **Result:** Perfect execution. Generated a clean, working minimal API.
- **The Catch:** Cost \$0.13 for a single generation. Scalability for rapid, trial-and-error prototyping is too expensive for this specific POC. Switched to local execution.

---

## Immediate Plans & Upcoming Skills

To completely clean up and modularize messy tutorial codebases, I am building out the following specific skill pipelines next:

- [ ] **net-model-migration:** Automatically extracts inline DTOs, records, and data models out of Program.cs or services and moves them into a dedicated Models folder.
- [ ] **net-di-setup:** Abstracts dependency injection setup out of Program.cs and into a dedicated ServiceExtensions.cs class using IServiceCollection extension methods.
- [ ] **net-service-layer:** Safely migrates business logic to separate class libraries or service directories without duplicating existing data structures or models.
