<!--
  Sync Impact Report
  ==================
  Version change: 0.1.0-draft → 1.0.0
  Bump rationale: MAJOR — first formal ratification of the constitution
  
  Modified principles:
    - (new) I. Interface-First Architecture
    - (new) II. Language Standardization
    - (new) III. .NET Framework Version Lock
    - (new) IV. XML Documentation
    - (new) V. Feature-Based Directory Organization
    - (new) VI. Input Validation
  
  Added sections:
    - Core Principles (6 principles)
    - Code Quality Patterns (property access, NuGet management)
    - Development Workflow (amendment process, review expectations)
    - Governance (amendment procedure, versioning, compliance)
  
  Removed sections: N/A (first formal version)
  
  Templates requiring updates:
    - .documentation/templates/plan-template.md ✅ no changes needed
      (Constitution Check section is dynamically populated)
    - .documentation/templates/spec-template.md ✅ no changes needed
      (requirements structure is compatible)
    - .documentation/templates/tasks-template.md ✅ no changes needed
      (task organization is compatible)
  
  Follow-up TODOs: None
-->

# WebProjectMechanics Constitution

## Core Principles

### I. Interface-First Architecture (MANDATORY)

All public types MUST implement corresponding interfaces. This
principle reflects 20+ years of architectural consistency in the
codebase.

- All public classes MUST implement a corresponding interface
- Interfaces MUST use the `I`-prefix naming convention
  (e.g., `ICompany` ↔ `Company`, `IArticle` ↔ `Article`)
- Internal and private implementations do not require interfaces

**Rationale**: Enables testability, supports dependency injection,
and improves API stability across the long-lived codebase.

### II. Language Standardization (MANDATORY)

New code MUST be written in C#. Existing VB.NET code is maintained
as-is; migration to C# is discretionary but not required.

- All NEW projects and files MUST be written in C#
- Existing VB.NET projects (WebProjectMechanics, wpmMineralCollection,
  WPMRecipe, LINQHelper) remain in maintenance mode
- Mixed-language solutions are acceptable where the VB.NET base remains

**Rationale**: C# has broader ecosystem adoption, richer tooling, and
reduces future friction. Respects existing VB.NET investment.

### III. .NET Framework Version Lock (MANDATORY)

All projects MUST target .NET Framework 4.8. No migration to
.NET Core / .NET 5+ without an explicit architecture decision.

- All `.csproj` and `.vbproj` files MUST specify
  `<TargetFramework>net48</TargetFramework>`
- `Web.config` compilation `targetFramework` MUST be `"4.8"`
- NuGet dependencies MUST be compatible with .NET Framework 4.8
- No .NET Standard or .NET 6+ targets without explicit team decision

**Rationale**: ASP.NET 4.8 hosting is stable and supported;
consistency across projects reduces build and deployment friction.

### IV. XML Documentation (MANDATORY)

All public types, properties, methods, and parameters MUST have
XML documentation comments.

- Every `public` class MUST have `/// <summary>`
- Every `public` method MUST have `/// <summary>`, `/// <param>`,
  and `/// <returns>`
- Every `public` property MUST have `/// <summary>`
- IDE warnings for missing documentation SHOULD be treated as
  PR review feedback
- Internal and private members do not require documentation

**Rationale**: Supports IntelliSense, auto-generated API docs, and
improves code discoverability for current and future contributors.

### V. Feature-Based Directory Organization (MANDATORY)

Code MUST be organized by feature, not by technical layer. Each
feature gets its own directory containing all related domain logic.

- New features MUST create a feature-named directory
- Feature directories MUST contain all classes related to that feature
  (interfaces, implementations, supporting types)
- Shared utilities go in `Utility/`, `Common/`, or `Services/`
  directories

**Rationale**: Improves navigation, encapsulation, and reduces
cognitive load when working within a feature boundary.

### VI. Input Validation (RECOMMENDED)

User input SHOULD be validated before processing. A centralized
validation approach is recommended but not yet mandated.

- All user-supplied data SHOULD be validated
- Validation SHOULD occur before business logic processing
- Security-sensitive inputs (SQL parameters, URLs, emails) MUST
  be validated regardless
- Validation errors SHOULD produce consistent error responses
- Framework choice is flexible (FluentValidation, DataAnnotations,
  custom)

**Rationale**: Prevents invalid data corruption and security
vulnerabilities; improves user experience and debuggability.

## Code Quality Patterns

### Strongly-Typed Property Access

Public types MUST expose data via properties (`Public Property` in
VB.NET or `public { get; set; }` in C#), not public fields.

### NuGet Package Management

Third-party dependencies MUST be managed via NuGet with
`packages.config` configuration. Direct assembly references to
unmanaged binaries SHOULD be avoided when a NuGet package exists.

## Development Workflow

### Pull Request Review

Every pull request MUST be reviewed against the principles in this
constitution before merge. Reviewers SHOULD use the following
checklist:

1. New public types implement interfaces (Principle I)
2. New code is written in C# (Principle II)
3. Target framework is .NET Framework 4.8 (Principle III)
4. Public API has XML documentation (Principle IV)
5. Code is organized in feature directories (Principle V)
6. User input is validated where applicable (Principle VI)

### Amendment Process

1. Proposed amendment documented in a GitHub issue or discussion
2. Tech lead evaluates impact on the project
3. Amendment approved or rejected with written rationale
4. If approved, constitution updated and team notified
5. Subsequent PR reviews validated against the updated principles

### Review Cycle

Amendments are made ad-hoc as the need arises. There is no forced
annual review cycle; changes are proposed when new patterns, tools,
or requirements emerge.

## Governance

This constitution supersedes all other project practices and
conventions. All pull requests and code reviews MUST verify
compliance with its principles. Any deviation MUST be justified
in the PR description with a rationale for the exception.

### Versioning Policy

The constitution version follows semantic versioning:

- **MAJOR**: Backward-incompatible governance changes, principle
  removals, or redefinitions
- **MINOR**: New principle or section added, or materially expanded
  guidance
- **PATCH**: Clarifications, wording, typo fixes, non-semantic
  refinements

### Compliance Review

Constitution compliance is validated during PR review. Automated
enforcement (e.g., Roslyn analyzers for documentation, interface
checks) SHOULD be adopted when feasible.

**Version**: 1.0.0 | **Ratified**: 2026-02-16 | **Last Amended**: 2026-02-16
