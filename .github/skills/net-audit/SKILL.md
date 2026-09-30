---
name: net-audit
description: Audits code against internal C# and API prefix routing standards
---

# Instructions
1. Inspect all API route templates in the target file.
2. If any endpoint string lacks the "tfc-" prefix, fail the audit and rewrite the code to include it.
3. Verify C# 12 primary constructor usage and ensure public methods have XML docs.
