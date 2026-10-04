# Copilot Instructions, Skills, and Agents POC

> A continuous log and framework for forcing AI code generation tools to refactor legacy tutorial code under strict workspace constraints—without writing a single line of manual code.

## The Core Philosophy & Constraint
* **Zero Manual Coding:** I am not creating folders, changing variables, or writing code. Everything outside of the .github folder is handled exclusively by the AI.
* **Prompt-Driven Engineering:** If the AI fails, I do not fix the code. I roll back the changes in Git, refine my prompts, custom instructions, or skills, and run it again.
* **The Ultimate Goal:** To iterate until the workflow is flawless, resulting in a reusable, plug-and-play set of instructions, skills, and agents that can automatically refactor tightly coupled tutorial code across any .NET project.

---

## The Workspace Strategy

### 1. Custom Instructions (.github/copilot-instructions.md)
Custom instructions enforce coding standards, language patterns, or architectural rules across the entire solution.

* **Setup Process:**
  1. Created .github/copilot-instructions.md in the root folder.
  2. Defined explicit behavior rules using Markdown.
  3. Enabled the feature in Visual Studio via: Tools > Options > GitHub > Copilot > Copilot Chat -> Check "Enable custom instructions to be loaded from .github/copilot-instructions.md files".
* **Status:** Currently Experimenting. I injected an explicit rule to preface all endpoints with tfc- as a visual test to verify if the instructions are being loaded and actively respected by the local model.

### 2. Copilot Skills (.github/skills/*/SKILL.md)
Skills are targetable behavior routines that can be explicitly invoked in the chat window.

* **LLM Context Constraints:** 
  * Failed: qwen3:8b failed to pick up custom skills (likely due to a small context window).
  * Passed: qwen3-coder:30b successfully parses and executes custom skills.
* **Early Architectural Lesson:** When tasked with refactoring endpoints into a dedicated Services folder, the model successfully implemented Dependency Injection (DI) but jammed the configuration directly into Program.cs and duplicated models. Lesson learned: Rules must explicitly restrict model duplication and define clean DI registration habits.

---

## Tooling & Workflow Log
### Experiment 003: Creating Test Project using custom Prompt/SKILL/templates with Kimi K3  
Goals: 
1. I wanted a solid test suite to make sure things didn't break in the future
2. I wanted to try out templates for the files generated in a SKILL
I had AI generate the skill and the templates.  It didn't work out _exactly_ as I wanted, but I wanted to move fast and figured I'd redo this step a few times.  

I started with my local `Qwen3-coder` model, but it took forever and I eventually stopped it.

Kimi really did a great job, but I realized my initial folder structure was naive.
> I'm documenting this step, but starting over for the next step


### Experiment 002: Ollama + BYOM (Free/Local)
Using qwen3-coder:30b, I created a targeted skill file to test adherence to strict standards:

* **File Created:** .github/skills/audit/net-audit.md
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
* Changed /analyze to /tfc-analyze
* Injected structured XML comments
* Preserved C# 12 primary constructor patterns on records

#### Test Case B: Generating New Features via Skills
* **Prompt Provided:**
  ```text
  /net-audit Create a new HTTP POST endpoint called "/sentiment" that accepts a text payload. It must evaluate the string and return a basic sentiment score (Positive, Negative, Neutral). Ensure the code strictly adheres to all our .github workspace rules.
  ```
* **Result:** Success. It generated a brand new /tfc-sentiment endpoint that natively adopted the tfc- prefix rule from the skill architecture and correctly guessed a basic scoring payload structure.

### Experiment 001: Claude Code (Paid/Cloud)
* **Prompt Provided:**
  ```text
  Create a new ASP.NET Core Minimal API project in this folder. Add one endpoint, a POST to /analyze, that accepts a JSON body with a "text" field, and returns the word count and character count of that text as JSON.
  ```
* **Result:** Perfect execution. Generated a clean, working minimal API.
* **The Catch:** Cost \$0.13 for a single generation. Scalability for rapid, trial-and-error prototyping is too expensive for this specific POC. Switched to local execution.

---

## Immediate Plans & Upcoming Skills
To completely clean up and modularize messy tutorial codebases, I am building out the following specific skill pipelines next:
- [ ] **net-model-migration:** Automatically extracts inline DTOs, records, and data models out of Program.cs or services and moves them into a dedicated Models folder.
- [ ] **net-di-setup:** Abstracts dependency injection setup out of Program.cs and into a dedicated ServiceExtensions.cs class using IServiceCollection extension methods.
- [ ] **net-service-layer:** Safely migrates business logic to separate class libraries or service directories without duplicating existing data structures or models.
