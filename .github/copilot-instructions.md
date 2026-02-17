# Copilot Instructions — WPM Greenfield

## Project Context

This is a greenfield rebuild of a 20-year-old multi-tenant CMS. The `archive/` folder contains the frozen legacy codebase (VB.NET / .NET Framework 4.8 / MS Access). All new code goes in `src/`, `tools/`, `tests/`, `admin/`, and `deploy/`.

## Code Generation Guidelines

### Language & Framework
- **C# 12+** targeting **.NET 9**
- **ASP.NET Core Minimal APIs** — not MVC controllers
- **EF Core 9** with SQLite provider
- Use file-scoped namespaces, primary constructors, records for DTOs
- Prefer `async/await` throughout

### Architecture
- Each content domain (CMS, Minerals, Recipes) is a separate project in `src/WPM.Domain.*`
- Domain projects reference only `WPM.Core` — never each other
- No `TenantId` columns — each site has its own SQLite `.db` files in a folder
- Domain `DbContext` classes take a `string dbPath` constructor parameter
- All SQLite connections must enable WAL mode

### Naming Conventions
- Pascal case for public members, camelCase for local variables
- Suffix `DbContext` classes with `DbContext` (e.g., `CmsDbContext`)
- Suffix endpoint classes with `Endpoints` (e.g., `LocationEndpoints`)
- Suffix publisher classes with `Publisher` (e.g., `CmsPublisher`)
- Use `Location` (not `Page`) for the CMS page entity — even though the legacy DB table is `Page`

### Testing
- xUnit with `[Fact]` and `[Theory]`
- Use in-memory SQLite for integration tests
- Test project naming: `WPM.{Project}.Tests`

### What to Avoid
- Do not generate code that references or modifies anything in `archive/`
- Do not add TenantId/CompanyId filtering to domain entity queries
- Do not use `Activator.CreateInstance` for DbContext creation in production code — use factory methods
- Do not generate React/TypeScript code unless specifically working in the `admin/` directory
- Do not add XML doc comments to every method — only where the intent isn't obvious from naming

### Documentation References
- Implementation plan: `.documentation/GREENFIELD_IMPLEMENTATION.md`
- Legacy system spec: `.documentation/MULTI_DOMAIN_ARCHITECTURE.md`
- AI assistant context: `CLAUDE.md` (root)
