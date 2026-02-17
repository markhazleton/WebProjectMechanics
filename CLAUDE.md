# CLAUDE.md — WPM Greenfield

This file provides context to Claude Code (and other AI assistants) working on this project.

## Project Overview

**Web Project Mechanics (WPM)** is a multi-tenant, multi-domain content management system being rebuilt from a legacy ASP.NET Web Forms / VB.NET / MS Access application into a modern .NET 9 / SQLite / static-site-publishing architecture.

- **Goal:** Host 36+ independent websites from a single codebase, publishing static HTML served by Caddy
- **Architecture:** Plugin-based content domains (CMS, Minerals, Recipes) with per-site SQLite databases
- **Key design principle:** Folder = Site = Isolation. No TenantId columns. Each site is a folder with its own `.db` files.

## Repository Structure

```
WebProjectMechanics/
├── archive/                 # FROZEN legacy code — read-only reference
│   ├── WebProjectMechanics/ # Legacy VB.NET core library
│   ├── website/             # Legacy web app + .mdb data + XML configs
│   ├── wpmMineralCollection/# Legacy mineral collection (LINQ to SQL)
│   ├── WPMRecipe/           # Legacy recipe module (LINQ to SQL)
│   └── WebProjectMechanics.sln
│
├── src/                     # NEW greenfield C# code
│   ├── WPM.Core/            # Shared contracts and models (no dependencies)
│   ├── WPM.Infrastructure/  # Core services (site resolution, publishing, templates)
│   ├── WPM.Api/             # ASP.NET Core 9 Minimal API host
│   ├── WPM.Domain.CMS/     # CMS content domain (locations, articles, parts)
│   ├── WPM.Domain.Minerals/ # Mineral collection domain
│   └── WPM.Domain.Recipes/  # Recipe domain
│
├── tools/WPM.Migration/     # One-time Access → SQLite migration tool
├── tests/                   # xUnit test projects
├── admin/                   # React admin SPA (future)
├── deploy/                  # Caddyfile, systemd unit, deploy scripts
├── .documentation/          # Architecture docs and implementation plan
└── WPM.sln                  # NEW solution file (src/ + tools/ + tests/)
```

## Technology Stack

| Layer | Technology |
|-------|-----------|
| Runtime | .NET 9 / ASP.NET Core Minimal APIs / C# |
| Database | SQLite via EF Core 9 (one DB per content domain per site) |
| Templating | Scriban (static HTML generation) |
| Web Server | Caddy 2 (static files + reverse proxy + auto-SSL) |
| Admin UI | TBD (Razor Pages MVP or React 19 + Vite) |
| Hosting | Azure Linux VM (Ubuntu 24.04, ~$10/month) |

## Key Conventions

### Code Style
- C# 12+ with file-scoped namespaces, primary constructors where appropriate
- Minimal APIs (not controllers) — use `MapGroup` / `MapGet` / `MapPost` patterns
- Records for DTOs and immutable data
- EF Core with code-first models
- No `TenantId` columns — tenant isolation is via separate `.db` files per site folder

### Database
- **All SQLite connections MUST use WAL mode:** `PRAGMA journal_mode=WAL; PRAGMA busy_timeout=5000;`
- Each content domain owns its own `DbContext` pointing to a specific `.db` file
- `CoreDbContext` is the only shared database (sites, users, auth)
- Domain `DbContext` instances are created per-request using the resolved site folder path

### Architecture Rules
- **Do not modify anything in `archive/`.** It is frozen reference material.
- Content domain code goes in its own `WPM.Domain.*` project
- Domain projects must not reference each other — only `WPM.Core`
- `WPM.Infrastructure` must not reference any `WPM.Domain.*` project
- `WPM.Api` references everything and wires it together in `Program.cs`
- Build the CMS domain as a direct integration first; extract plugin interfaces later

### Testing
- xUnit for all tests
- Test projects mirror src/ structure: `WPM.Core.Tests`, `WPM.Domain.CMS.Tests`, etc.
- Use in-memory SQLite for integration tests

## Key Documentation

- [GREENFIELD_IMPLEMENTATION.md](.documentation/GREENFIELD_IMPLEMENTATION.md) — Full implementation plan (start here)
- [MULTI_DOMAIN_ARCHITECTURE.md](.documentation/MULTI_DOMAIN_ARCHITECTURE.md) — Legacy system specification
- [QUICK_REFERENCE.md](.documentation/QUICK_REFERENCE.md) — Legacy system quick reference

## Migration Context

The migration tool (`tools/WPM.Migration`) reads from three sources:
1. **16 .mdb files** in `archive/website/App_Data/` — CMS content (Company, Page, Article, etc.)
2. **SQL scripts** (provided separately) — Mineral collection and recipe data from defunct Azure SQL / AWS RDS
3. **Legacy IIS server filesystem** — Physical image files organized by `Company.GalleryFolder`

**Important legacy naming:** The legacy database table is called `Page` (not `Location`). The `Location` class in the legacy VB.NET code is an in-memory entity. The new system uses `Location` as the entity name, mapping from the legacy `Page` table.

## What NOT to Do

- Do not add Entity Framework migrations to archive/ projects
- Do not attempt to run the legacy `WebProjectMechanics.sln` — it requires .NET Framework 4.8 + IIS + ACE OLE DB
- Do not add TenantId/CompanyID columns to new domain entities — physical file isolation handles this
- Do not build plugin infrastructure (IContentDomain, reflection discovery) until 2+ domains are working
- Do not build the React admin SPA until the publishing pipeline is proven
- Do not clean legacy HTML content during migration — migrate verbatim, iterate later
