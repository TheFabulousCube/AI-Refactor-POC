---
name: update-readme
description: Update the README.md with a new entry in the Tooling & Workflow Log.
---

Document each agent-made repository change in the `README.md` workflow log.

## Target Section
"N:\C# Playground\the-true-code\Copilot Test\README.md"

Update:

```md
## Tooling & Workflow Log
```

Insert each new entry directly below this heading, above prior entries.

## Entry Format - Template

Use the following template for each new entry:

`.github/skills/update-readme/templates/readme-log-entry.md`

The template uses the following placeholders:

- `{{EXPERIMENT_NUMBER}}` - Three-digit experiment number (e.g., 007)
- `{{DESCRIPTIVE_TITLE}}` - Brief description of what was accomplished
- `{{MODEL_NAME}}` - Name of the AI model used (e.g., qwen3:8b, qwen3-coder:30b)
- `{{GOALS}}` - Bulleted list restating the user request and skills used
- `{{WHAT_WORKED}}` - Numbered list of actions the agent performed
- `{{KEY_LEARNINGS}}` - Problems encountered, things to watch out for, or insights gained

Copy the template, replace all placeholders with actual content, and insert the completed entry directly below:

```md
## Tooling & Workflow Log
```

## Procedure

1. Open `README.md`.
2. Find `## Tooling & Workflow Log`.
3. Find the highest existing experiment number.
4. Add the next three-digit number, such as `Experiment 004`.
5. Insert the new entry at the top of the section.
6. Keep the entry factual and concise.
7. Do not remove or rewrite older entries unless asked.

## Final Response

Mention the README update:

```md
Updated `README.md` with `Experiment NNN` in the `Tooling & Workflow Log`.
```
