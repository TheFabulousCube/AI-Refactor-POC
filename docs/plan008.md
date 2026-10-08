# Refactor TextAnalyzer.Api to use Services

## Goal
Refactor the business logic in `Program.cs` into separate services and use Dependency Injection.

## Plan

### Phase 1: Preparation
- [x] Verify initial build and tests pass.

### Phase 2: Service Implementation
- [x] Define `ITextAnalysisService` and `ITextAnalysisService` interfaces.
- [x] Create `TextAnalysisService` implementation.
- [x] Create `SentimentAnalysisService` implementation.
- [x] Create a new folder `src/TextAnalyzer.Api/Services/`.

### Phase 3: Dependency Injection
- [x] Create `ServiceExtensions.cs` in `src/TextAnalyzer.Api/`.
- [x] Implement `AddTextAnalyzerServices` extension method in `ServiceExtensions.cs`.
- [x] Register services in `Program.cs` using the extension method.

### Phase 4: Refactor Program.cs
- [x] Update `Program.cs` endpoints to use the new services.

### Phase 5: Verification
- [x] Run build.
- [x] Run tests.
- [x] Update README.md (if necessary).
