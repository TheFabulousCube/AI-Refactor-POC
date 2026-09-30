---
name: refactor-to-service
description: Extracts inline endpoint execution logic into decoupled C# Service classes
---

# Instructions
1. Analyze the targeted API endpoint method block.
2. Create a new service interface (e.g., `ITextService`) with asynchronous method signatures.
3. Implement a corresponding Service class using modern C# 12+ primary constructors.
4. Move all validation, mapping, and heavy processing logic into the new service.
5. Update the Minimal API route map or Controller action to accept the interface via dependency injection.
6. Return clean `Results` or HTTP status wrappers from the endpoint while keeping it ultra-thin.
