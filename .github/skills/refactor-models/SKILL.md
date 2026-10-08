---
name: refactor-models
description: Moves data contracts, requests, responses, and records to the Models folder.
---

# Instructions

1. Locate all entity schemas, DTOs, records, or input classes in the target code.
2. Relocate them to standalone files inside the project's root `/Models` directory.
3. Create `/Models` if it doesn't exist.
4. Use a single class per file, with the filename matching the class name.
5. Combine small related classes into a single file only if they are tightly coupled and unlikely to be used independently.
6. Update their namespaces to append `.Models`.
7. Update all references to these types throughout the codebase to reflect their new namespace and location.
8. Don't forget to update tests
9. Do not make any other changes
