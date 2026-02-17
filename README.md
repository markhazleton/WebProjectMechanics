# Web Project Mechanics — Greenfield Rebuild

A multi-tenant, multi-domain content management system that publishes static HTML websites from SQLite databases.

**Status:** In development — migrating from a 20+ year legacy ASP.NET Web Forms / MS Access system to .NET 9 / SQLite / Caddy.

## Architecture

- **36+ domains** served from a single application instance
- **Plugin-based content domains** — CMS pages, Mineral Collection, Recipes
- **Per-site SQLite databases** — no TenantId columns, physical file isolation
- **Publish-to-static** — all public content is pre-rendered HTML served by Caddy
- **Single VM** — ~$10/month on Azure Linux

## Repository Structure

```
├── archive/             # Frozen legacy codebase (read-only reference)
├── src/                 # New .NET 9 greenfield code
│   ├── WPM.Core/        # Shared contracts and models
│   ├── WPM.Infrastructure/  # Core services
│   ├── WPM.Api/         # ASP.NET Core Minimal API host
│   ├── WPM.Domain.CMS/  # CMS content domain
│   ├── WPM.Domain.Minerals/  # Mineral collection domain
│   └── WPM.Domain.Recipes/   # Recipe domain
├── tools/WPM.Migration/ # One-time data migration tool
├── tests/               # xUnit test projects
├── admin/               # Admin UI (future)
├── deploy/              # Deployment config (Caddy, systemd)
└── .documentation/      # Architecture docs and implementation plan
```

## Documentation

- **[Implementation Plan](.documentation/GREENFIELD_IMPLEMENTATION.md)** — Full technical specification and phased plan
- **[Legacy System Spec](.documentation/MULTI_DOMAIN_ARCHITECTURE.md)** — Complete documentation of the current system
- **[Quick Reference](.documentation/QUICK_REFERENCE.md)** — Legacy system at a glance

## Getting Started

*Coming soon — Phase 1 (Foundation) is in progress.*

```bash
# Build the API
dotnet build src/WPM.Api/

# Run tests
dotnet test

# Run the API locally
dotnet run --project src/WPM.Api/
```

## Technology Stack

| Layer | Technology |
|-------|-----------|
| API | ASP.NET Core 9 Minimal APIs (C#) |
| Database | SQLite via EF Core 9 |
| Templating | Scriban |
| Web Server | Caddy 2 (auto-SSL, static files, reverse proxy) |
| Hosting | Azure Linux VM (Ubuntu 24.04) |
| CI/CD | GitHub Actions |

## License

MIT — See [LICENSE](LICENSE) for details.
