# WPM Greenfield — Technology Implementation Plan

**Version:** 0.4 (Reviewed)
**Date:** February 2026
**Status:** In Progress — Plan reviewed, blocking questions resolved
**Companion to:** [MULTI_DOMAIN_ARCHITECTURE.md](MULTI_DOMAIN_ARCHITECTURE.md) (current system spec)

---

## Table of Contents

1. [Design Goals](#1-design-goals)
2. [Architecture Overview](#2-architecture-overview)
3. [Technology Stack](#3-technology-stack)
4. [Plugin Architecture — Content Domains](#4-plugin-architecture--content-domains)
5. [Core Contracts](#5-core-contracts)
6. [Solution Structure](#6-solution-structure)
7. [Data Layer](#7-data-layer)
8. [API Layer](#8-api-layer)
9. [Publishing Pipeline](#9-publishing-pipeline)
10. [Admin UI](#10-admin-ui)
11. [Hosting & Infrastructure](#11-hosting--infrastructure)
12. [Multi-Domain Routing (Caddy)](#12-multi-domain-routing-caddy)
13. [Migration from Legacy System](#13-migration-from-legacy-system)
14. [Gap Analysis](#14-gap-analysis)
15. [Risk Register](#15-risk-register)
16. [Implementation Phases](#16-implementation-phases)
17. [Open Questions](#17-open-questions)

---

## 1. Design Goals

| Goal | Description |
|------|-------------|
| **Plugin-based content domains** | Minerals, Recipes, CMS pages, and future content types plug in without modifying core code |
| **Publish-to-static** | All public-facing content is pre-rendered HTML served from disk — no runtime database queries for visitors |
| **Single VM, minimal cost** | Everything runs on one cheap Linux VM (~$10/month) — no PaaS sprawl |
| **Single codebase, single language** | C# / .NET 9 end-to-end for API, publishing, and templates |
| **Multi-tenant** | 36+ domains served from one application instance |
| **Per-domain databases** | Each content domain (CMS, Minerals, Recipes) owns its own SQLite file per site; core platform has a separate DB for sites/auth |
| **CMS integration contract** | Every domain database must implement a standard contract (required tables/columns) validated at startup |
| **Simple operations** | SQLite file databases, Caddy web server, systemd service — no container orchestration |
| **Admin separation** | React SPA for content editing, completely decoupled from public sites |

---

## 2. Architecture Overview

```
┌──────────── Internet (36+ domains) ──────────────┐
│  frogsfolly.com, jmshawminerals.com, etc.         │
└────────────────────┬──────────────────────────────┘
                     ▼
┌──────────────────────────────────────────────────────────────┐
│  Azure Linux VM (B1s: 1 vCPU, 1 GB RAM, ~$7/mo)             │
│                                                               │
│  ┌────────────────────────────────────────────────────────┐   │
│  │  Caddy web server                                      │   │
│  │  • Auto-SSL (Let's Encrypt) for ALL domains            │   │
│  │  • Serves static HTML from /var/wpm/sites/{domain}/    │   │
│  │  • Reverse proxy /api/* → localhost:5000                │   │
│  │  • www → non-www 301 redirects                         │   │
│  └────────────────────────────────────────────────────────┘   │
│                                                               │
│  ┌────────────────────────────────────────────────────────┐   │
│  │  ASP.NET Core 9 API  (systemd service, localhost:5000) │   │
│  │                                                         │   │
│  │  Core Platform                                          │   │
│  │  ├── Site resolution (Host header → site folder)       │   │
│  │  ├── Authentication (JWT)                               │   │
│  │  ├── Publishing pipeline (orchestrates all domains)     │   │
│  │  ├── Schema validator (checks CMS integration contract)│   │
│  │  └── Domain registry (auto-discovers plugins)           │   │
│  │                                                         │   │
│  │  Content Domain Plugins                                 │   │
│  │  ├── WPM.Domain.CMS       (Locations, Articles, Parts) │   │
│  │  ├── WPM.Domain.Minerals  (Specimens, Minerals)        │   │
│  │  ├── WPM.Domain.Recipes   (Recipes, Ingredients)       │   │
│  │  └── WPM.Domain.???       (future — plug and play)     │   │
│  └──────────────────────────────────────────────────────────┘   │
│                                                               │
│  ┌────────────────────────────────────────────────────────┐   │
│  │  /var/wpm/                                              │   │
│  │  ├── core.db                (Shared: sites, users, auth)│   │
│  │  │                                                      │   │
│  │  ├── data/                  (Source data — per site)    │   │
│  │  │   ├── frogsfolly.com/                                │   │
│  │  │   │   ├── cms.db         (CMS content for this site) │   │
│  │  │   │   └── media/         (images for this site)      │   │
│  │  │   ├── jmshawminerals.com/                            │   │
│  │  │   │   ├── cms.db         (CMS content)               │   │
│  │  │   │   ├── minerals.db    (mineral collection)        │   │
│  │  │   │   └── media/         (specimen photos)           │   │
│  │  │   └── mechanicsofmotherhood.com/                     │   │
│  │  │       ├── cms.db         (CMS content)               │   │
│  │  │       ├── recipes.db     (recipes)                   │   │
│  │  │       └── media/         (recipe photos)             │   │
│  │  │                                                      │   │
│  │  └── sites/                 (Published static output)   │   │
│  │      ├── frogsfolly.com/    (HTML served by Caddy)      │   │
│  │      ├── jmshawminerals.com/                            │   │
│  │      ├── admin/             (React SPA)                 │   │
│  │      └── ...                                            │   │
│  └────────────────────────────────────────────────────────┘   │
└──────────────────────────────────────────────────────────────┘
```

### Key Insight: Folder = Site = Isolation

Each site is a **folder**. Inside it, each enabled content domain plugin creates its own SQLite `.db` file. There are no `TenantId` columns anywhere — **physical file isolation replaces logical isolation**. This mirrors the legacy architecture where each domain had its own `.mdb` file, but with a cleaner structure.

- **Backup a site** = zip the folder
- **Delete a site** = delete the folder
- **Clone a site** = copy the folder, rename
- **No cross-site data leaks** = impossible by design (separate files)
- **Each domain's DB is independently manageable** = you can open `minerals.db` in any SQLite browser

---

## 3. Technology Stack

| Layer | Technology | Rationale |
|-------|-----------|-----------|
| **OS** | Ubuntu 24.04 LTS | Cheapest Azure VM tier; .NET 9 runs natively |
| **Web Server** | Caddy 2 | Auto-SSL for 36+ domains; static file serving; reverse proxy; minimal config |
| **API** | ASP.NET Core 9 Minimal APIs (C#) | Natural migration from VB.NET; strong multi-tenancy support; EF Core integration |
| **Database** | SQLite via EF Core 9 (one DB per content domain per site) | Each domain owns its own `.db` file per site folder; core platform has separate DB for sites/auth; matches Access scale; zero cost; independently backupable |
| **Auth** | ASP.NET Core Identity + JWT | Built-in; maps to current user roles (Admin/Editor/User/Anonymous) |
| **Templating** | Scriban (or RazorLight) | Lightweight .NET template engine for static HTML generation; no Node.js dependency |
| **Admin UI** | React 19 + Vite + TypeScript | Modern SPA, separate from public sites; calls API exclusively |
| **CSS** | Tailwind CSS | Utility-first; pre-compiled during CI; no runtime CSS processing |
| **CI/CD** | GitHub Actions | Free tier (2000 min/month); builds API, admin, and triggers publish |
| **Hosting** | Azure Linux VM (B1s) | ~$7/month; persistent disk for SQLite + static files |

### What We're NOT Using (and Why)

| Avoided | Why |
|---------|-----|
| Azure App Service / Static Web Apps | Per-app cost adds up with 36 domains; overkill for static file serving |
| Azure SQL / Cosmos DB | SQLite handles this scale; no need for a database server |
| Astro / Next.js / Hugo | Publishing is done in C# by each content domain plugin — no Node.js SSG needed |
| Docker / Kubernetes | One process + one web server on one VM; containers add complexity without benefit here |
| Azure Front Door / CDN | Caddy on-VM serves static files fast enough; CDN can be added later if needed |
| Nginx | Caddy's auto-SSL for 36 domains is dramatically simpler |

---

## 4. Plugin Architecture — Content Domains

### Concept

Every content type (CMS pages, Minerals, Recipes, future domains) is a **plugin** that implements a standard set of interfaces. The core platform discovers, registers, and orchestrates plugins without knowing their internals.

Each plugin **owns its own SQLite database file** per site. When the platform starts or a site is accessed, it:

```
Core Platform (knows nothing about minerals or recipes)
     │
     ├── Discovers all IContentDomain implementations at startup
     │
     ├── For each site folder in /var/wpm/data/{domain}/:
     │   ├── Checks which .db files exist (cms.db, minerals.db, etc.)
     │   ├── Validates each DB against its domain's schema contract
     │   └── Reports any schema violations at startup (fail-fast)
     │
     ├── Calls MapEndpoints() → registers API routes per domain
     ├── Calls ConfigureServices() → registers DI services per domain
     │
     └── At publish time:
         ├── For each site, finds which domains are enabled (which .db files exist)
         ├── Opens each domain's DB from the site folder
         └── Calls PublishAsync() → domain reads its own DB, writes HTML to output
              → Caddy serves them immediately
```

### Adding a New Content Domain

To add a new content type (e.g., "Photography Portfolio"):

1. Create project `WPM.Domain.Photography`
2. Implement `IContentDomain` — define DB filename (`photography.db`), schema, endpoints
3. Implement `IContentPublisher` — HTML generation from templates
4. Implement `ISchemaContract` — declare required tables/columns for validation
5. Add Scriban templates for public-facing pages
6. Add React admin module under `admin/src/modules/photography/`
7. Reference the project from `WPM.Api`
8. To enable on a site: create `photography.db` in `/var/wpm/data/{domain}/`
9. Run schema migration: `dotnet wpm migrate --site jmshawminerals.com --domain photography`

**No core code changes required. A site enables a domain by having the `.db` file in its folder.**

### Implementation Strategy: Pragmatic Plugin Extraction

> **Important:** Don't build the full plugin infrastructure on day one. The reflection-based
> discovery, schema validation, and `IContentDomain` interface should emerge from working code,
> not precede it.
>
> **Phase 1:** Build `WPM.Domain.CMS` as a direct, hardcoded integration. No interfaces, no
> reflection. Just get sites publishing.
>
> **Phase 2:** When adding `WPM.Domain.Minerals`, extract the common patterns into interfaces.
> Now you have two concrete implementations to guide the abstraction.
>
> **Phase 3:** Formalize `IContentDomain`, `ISchemaContract`, and reflection-based discovery
> once the pattern is proven across 2+ domains.
>
> This avoids spending weeks on plugin infrastructure before a single page is published.

---

## 5. Core Contracts

### IContentDomain

```csharp
/// <summary>
/// Every content domain implements this. The platform discovers
/// and registers all implementations at startup via reflection.
/// Each domain owns its own SQLite database file per site.
/// </summary>
public interface IContentDomain
{
    /// Unique identifier: "cms", "minerals", "recipes"
    string DomainId { get; }

    /// Display name for admin UI: "Content Management", "Mineral Collection"
    string DisplayName { get; }

    /// The SQLite filename this domain uses: "cms.db", "minerals.db"
    string DatabaseFileName { get; }

    /// Create a DbContext for this domain, pointed at a specific site's DB file.
    /// Called per-request with the resolved site path.
    DbContext CreateDbContext(string databasePath);

    /// Register this domain's EF Core model (used for migrations and validation)
    void ConfigureModel(ModelBuilder modelBuilder);

    /// Register this domain's Minimal API endpoints
    void MapEndpoints(IEndpointRouteBuilder routes);

    /// Register this domain's services in the DI container
    void ConfigureServices(IServiceCollection services);

    /// Return the CMS integration contract for this domain's database.
    /// Used by the platform to validate that the DB has required tables.
    ISchemaContract GetSchemaContract();
}
```

### ISchemaContract (CMS Integration Validation)

```csharp
/// <summary>
/// Defines the required schema that a domain's database must implement.
/// The platform validates every .db file against this contract at startup.
/// If validation fails, the site/domain is flagged and won't publish.
/// </summary>
public interface ISchemaContract
{
    /// Domain this contract belongs to
    string DomainId { get; }

    /// Tables that MUST exist in the database
    IReadOnlyList<RequiredTable> RequiredTables { get; }

    /// Validate a specific database file against this contract.
    /// Returns a list of violations (empty = valid).
    Task<IReadOnlyList<SchemaViolation>> ValidateAsync(string databasePath);
}

/// <summary>
/// Describes a required table and its required columns.
/// </summary>
public record RequiredTable(
    string TableName,
    IReadOnlyList<RequiredColumn> Columns
);

public record RequiredColumn(
    string ColumnName,
    string ExpectedType,       // "TEXT", "INTEGER", "REAL", "BLOB"
    bool IsRequired             // NOT NULL constraint expected
);

public record SchemaViolation(
    string TableName,
    string? ColumnName,
    string Message              // "Table 'ContentItems' not found"
);
```

**Every content domain must declare a schema contract.** The CMS domain's contract requires tables like `Locations`, `Articles`, etc. The Minerals domain requires `Specimens`, `Minerals`, etc. At startup, the platform opens each site's `.db` files and validates them:

```csharp
// Startup validation (in Program.cs or a hosted service)
foreach (var siteFolder in Directory.GetDirectories("/var/wpm/data"))
{
    var siteName = Path.GetFileName(siteFolder);
    foreach (var domain in domains)
    {
        var dbPath = Path.Combine(siteFolder, domain.DatabaseFileName);
        if (!File.Exists(dbPath)) continue; // domain not enabled for this site

        var contract = domain.GetSchemaContract();
        var violations = await contract.ValidateAsync(dbPath);

        if (violations.Any())
        {
            logger.LogError("Site {Site} domain {Domain}: {Count} schema violations",
                siteName, domain.DomainId, violations.Count);
            foreach (var v in violations)
                logger.LogError("  {Table}.{Column}: {Message}",
                    v.TableName, v.ColumnName, v.Message);
        }
        else
        {
            logger.LogInformation("Site {Site} domain {Domain}: schema valid ✓",
                siteName, domain.DomainId);
        }
    }
}
```

### IContentPublisher

```csharp
/// <summary>
/// The publishing contract. Each domain knows how to transform its
/// data into static output files (HTML, JSON, XML, RSS).
/// The publisher reads from its own DB in the site folder.
/// </summary>
public interface IContentPublisher
{
    /// Which domain this publisher belongs to
    string DomainId { get; }

    /// Generate ALL publishable content for a site.
    /// The SiteContext provides the path to the site's data folder.
    Task<IReadOnlyList<PublishableFile>> PublishAsync(
        SiteContext context,
        CancellationToken ct = default);

    /// Generate content for a single item change (incremental publish).
    Task<IReadOnlyList<PublishableFile>> PublishItemAsync(
        SiteContext context,
        string itemId,
        CancellationToken ct = default);
}
```

### IAdminModule

```csharp
/// <summary>
/// Describes the admin UI module for this domain.
/// The React admin shell uses this metadata to build navigation
/// and lazy-load the correct module.
/// </summary>
public interface IAdminModule
{
    string DomainId { get; }

    /// Navigation items to show in the admin sidebar
    IReadOnlyList<AdminNavItem> NavItems { get; }

    /// Route prefix in the admin SPA: "/admin/minerals"
    string AdminRoutePrefix { get; }
}
```

### Supporting Types

```csharp
public record PublishableFile(
    string RelativePath,   // "collection/quartz/index.html"
    string Content,        // HTML string
    string ContentType     // "text/html", "application/xml"
);

/// <summary>
/// Passed to publishers and domain services. Identifies the current site
/// and provides paths to its data folder and output folder.
/// </summary>
public record SiteContext(
    string SiteDomain,       // "jmshawminerals.com"
    string DataFolder,       // "/var/wpm/data/jmshawminerals.com"
    string OutputFolder,     // "/var/wpm/sites/jmshawminerals.com"
    string MediaFolder,      // "/var/wpm/data/jmshawminerals.com/media"
    SiteConfig Config,       // Site-level config (theme, home page, etc.)
    IServiceProvider Services
);

public record AdminNavItem(
    string Label,          // "Specimens"
    string Icon,           // "gem" (icon name)
    string Route,          // "/admin/minerals/specimens"
    int SortOrder
);
```

---

## 6. Solution Structure

> **Updated Feb 2026:** The greenfield code lives alongside the archived legacy code in the
> same repository. The `archive/` folder is frozen — never modified. All new development
> happens in `src/`, `tools/`, `tests/`, `admin/`, and `deploy/`. The legacy `.mdb` data files
> in `archive/website/App_Data/` are directly accessible to the migration tool.

```
WebProjectMechanics/                         # Single repo — legacy + greenfield
│
├── CLAUDE.md                                # AI assistant context (Claude Code)
├── README.md                                # Project overview (greenfield-focused)
├── WPM.sln                                  # NEW solution (src/ + tools/ + tests/)
├── .gitignore                               # Updated for both legacy and greenfield
│
├── .documentation/                          # Architecture docs (shared)
│   ├── GREENFIELD_IMPLEMENTATION.md         # This plan
│   ├── MULTI_DOMAIN_ARCHITECTURE.md         # Legacy system spec
│   ├── QUICK_REFERENCE.md
│   └── VISUAL_DIAGRAMS.md
│
├── .github/
│   ├── copilot-instructions.md              # GitHub Copilot context
│   ├── agents/                              # SpecKit agents (existing)
│   ├── prompts/                             # SpecKit prompts (existing)
│   └── workflows/
│       ├── api.yml                          # Build + test + deploy API
│       ├── admin.yml                        # Build + deploy admin SPA
│       └── publish-sites.yml                # Trigger full site publish
│
├── archive/                                 # ══ FROZEN LEGACY CODE ══
│   ├── WebProjectMechanics.sln              # Legacy solution (do not modify)
│   ├── azure-pipelines.yml                  # Legacy CI (do not modify)
│   ├── WebProjectMechanics/                 # VB.NET core library
│   ├── website/                             # ASP.NET Web Forms app
│   │   └── App_Data/                        # ← .mdb files + XML configs live here
│   │       ├── *.mdb                        #   (16 databases, read by migration tool)
│   │       └── Sites/*.xml                  #   (37 domain configs)
│   ├── wpmMineralCollection/                # Mineral collection (LINQ to SQL)
│   ├── WPMRecipe/                           # Recipe module (LINQ to SQL)
│   ├── LINQHelper/                          # Dynamic LINQ helper
│   ├── LumenWorks.Framework.IO/             # CSV framework
│   ├── RemoveWWW/                           # HTTP module
│   └── RssToolkit/                          # RSS feed toolkit
│
├── src/                                     # ══ NEW GREENFIELD CODE ══
│   ├── WPM.Core/                            # Shared contracts, no dependencies
│   │   ├── Interfaces/
│   │   │   ├── IContentPublisher.cs         # (added in Phase 2, not Phase 1)
│   │   │   └── ISiteService.cs
│   │   ├── Models/
│   │   │   ├── Site.cs
│   │   │   ├── SiteDomain.cs
│   │   │   ├── User.cs
│   │   │   ├── SiteContext.cs
│   │   │   └── PublishableFile.cs
│   │   └── WPM.Core.csproj
│   │
│   ├── WPM.Infrastructure/                  # Core services
│   │   ├── Data/
│   │   │   ├── CoreDbContext.cs              # ONLY shared DB: Sites, Users, Auth
│   │   │   └── Migrations/
│   │   ├── Services/
│   │   │   ├── SiteService.cs               # Host → site folder resolution
│   │   │   ├── SiteDomainDetector.cs        # Scans .db files in site folder
│   │   │   ├── PublishingService.cs          # Orchestrates publishers
│   │   │   └── TemplateEngine.cs            # Scriban wrapper
│   │   └── WPM.Infrastructure.csproj
│   │
│   ├── WPM.Api/                             # ASP.NET Core 9 host
│   │   ├── Program.cs
│   │   ├── Middleware/
│   │   │   └── SiteMiddleware.cs            # Host header → SiteContext
│   │   ├── Endpoints/
│   │   │   ├── SiteEndpoints.cs
│   │   │   ├── AuthEndpoints.cs
│   │   │   └── PublishEndpoints.cs
│   │   └── WPM.Api.csproj
│   │
│   ├── WPM.Domain.CMS/                     # CMS content domain
│   │   ├── CmsDbContext.cs                  # DbContext for cms.db
│   │   ├── CmsPublisher.cs                 # Static HTML generator
│   │   ├── Models/
│   │   │   ├── Location.cs                 # Maps from legacy Page table
│   │   │   ├── Article.cs
│   │   │   ├── Part.cs
│   │   │   ├── Parameter.cs
│   │   │   ├── Image.cs
│   │   │   └── LocationAlias.cs            # URL redirects (from PageAlias)
│   │   ├── Endpoints/
│   │   │   ├── LocationEndpoints.cs
│   │   │   └── ArticleEndpoints.cs
│   │   ├── Templates/
│   │   │   ├── _layout.scriban
│   │   │   ├── page.scriban
│   │   │   ├── article.scriban
│   │   │   └── sitemap.scriban
│   │   └── WPM.Domain.CMS.csproj
│   │
│   ├── WPM.Domain.Minerals/                # Mineral collection domain
│   │   ├── MineralsDbContext.cs
│   │   ├── MineralsPublisher.cs
│   │   ├── Models/
│   │   │   ├── Specimen.cs                 # Maps from CollectionItem
│   │   │   ├── Mineral.cs
│   │   │   ├── SpecimenMineral.cs          # Many-to-many junction
│   │   │   └── SpecimenImage.cs
│   │   ├── Endpoints/
│   │   │   └── SpecimensEndpoints.cs
│   │   ├── Templates/
│   │   │   ├── collection.scriban
│   │   │   └── specimen.scriban
│   │   └── WPM.Domain.Minerals.csproj
│   │
│   └── WPM.Domain.Recipes/                 # Recipe domain
│       ├── RecipesDbContext.cs
│       ├── RecipesPublisher.cs
│       ├── Models/
│       │   ├── Recipe.cs
│       │   ├── RecipeImage.cs
│       │   ├── RecipeComment.cs
│       │   └── RecipeCategory.cs
│       ├── Endpoints/
│       │   └── RecipeEndpoints.cs
│       ├── Templates/
│       │   ├── recipe-list.scriban
│       │   └── recipe.scriban
│       └── WPM.Domain.Recipes.csproj
│
├── tests/                                   # xUnit test projects
│   ├── WPM.Core.Tests/
│   ├── WPM.Infrastructure.Tests/
│   ├── WPM.Api.Tests/
│   └── WPM.Domain.CMS.Tests/
│
├── tools/
│   └── WPM.Migration/                      # One-time: Access/SQL → per-site SQLite
│       ├── Program.cs                       # References archive/website/App_Data/*.mdb
│       ├── SchemaDiscovery.cs               # Phase 0: dump all .mdb schemas
│       ├── CmsMigrator.cs                   # Phase A: .mdb → cms.db per site
│       ├── MineralsMigrator.cs              # Phase B: SQL scripts → minerals.db
│       ├── RecipesMigrator.cs               # Phase C: SQL scripts → recipes.db
│       ├── UrlRedirectMapper.cs             # Generate Caddy redirect rules
│       └── WPM.Migration.csproj
│
├── admin/                                   # Admin UI (future — Phase 3+)
│
└── deploy/
    ├── Caddyfile                            # Auto-generated from core.db
    ├── wpm-api.service                      # systemd unit file
    └── deploy.sh                            # rsync-based deployment
```

### Dependency Rules

```
WPM.Api ──→ WPM.Infrastructure ──→ WPM.Core
  │                                    ↑
  ├──→ WPM.Domain.CMS ────────────────┘
  ├──→ WPM.Domain.Minerals ───────────┘
  └──→ WPM.Domain.Recipes ────────────┘

WPM.Domain.* projects NEVER reference each other.
WPM.Infrastructure NEVER references WPM.Domain.*.
WPM.Migration references archive/ data files (read-only) + src/ EF Core models.
```

---

## 7. Data Layer

### 7.1 Per-Site Folder Structure (No TenantId)

There is **no shared content database and no TenantId columns**. Instead, each site is a folder containing its own SQLite database files — one per enabled content domain. The core platform database is the only shared DB and only stores site registry, users, and authentication.

**Physical isolation replaces logical isolation.** This mirrors the legacy architecture where each company had its own `.mdb` file, but with a cleaner structure.

```
/var/wpm/
├── core.db                                    # SHARED: site registry, users, auth
│
├── data/                                      # SOURCE DATA: per-site folders
│   ├── frogsfolly.com/
│   │   ├── cms.db                             # CMS content (locations, articles, parts)
│   │   ├── site.json                          # Site config (theme, home page, etc.)
│   │   └── media/                             # Images for this site
│   │       ├── gallery/
│   │       └── logos/
│   │
│   ├── jmshawminerals.com/
│   │   ├── cms.db                             # CMS content
│   │   ├── minerals.db                        # Mineral collection (specimens, minerals)
│   │   ├── site.json                          # Site config
│   │   └── media/
│   │       ├── gallery/
│   │       └── specimens/                     # Mineral specimen photos
│   │
│   ├── mechanicsofmotherhood.com/
│   │   ├── cms.db                             # CMS content
│   │   ├── recipes.db                         # Recipes (recipes, ingredients)
│   │   ├── site.json                          # Site config
│   │   └── media/
│   │       └── recipes/                       # Recipe photos
│   │
│   └── webprojectmechanics.com/
│       ├── cms.db                             # CMS content
│       ├── site.json
│       └── media/
│
└── sites/                                     # PUBLISHED OUTPUT: served by Caddy
    ├── frogsfolly.com/                        # Static HTML
    ├── jmshawminerals.com/
    ├── mechanicsofmotherhood.com/
    ├── admin/                                 # React admin SPA
    └── ...
```

### Key Benefits

- **No TenantId anywhere** — content tables are clean, no global query filters needed
- **Backup a site** = `zip /var/wpm/data/frogsfolly.com/`
- **Delete a site** = `rm -rf /var/wpm/data/frogsfolly.com/`
- **Clone a site** = `cp -r /var/wpm/data/source/ /var/wpm/data/newsite/`
- **No cross-site data leaks** — impossible by design (separate files)
- **Each domain DB is independently browsable** — open `minerals.db` in any SQLite tool
- **Domain enablement is implicit** — if `minerals.db` exists in the folder, the minerals domain is active for that site

### 7.1.1 SQLite Concurrency: WAL Mode Required

The API serves admin CRUD (writes) and the publishing pipeline (reads) against the same SQLite files. SQLite allows only one writer at a time. Without configuration, concurrent access will cause `SQLITE_BUSY` errors.

**Solution:** Enable WAL (Write-Ahead Logging) mode on every SQLite connection:

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder options)
{
    options.UseSqlite($"Data Source={_dbPath}", sqliteOptions =>
    {
        // Connection-level: set busy timeout to wait instead of failing
    });
}

// On first connection to any DB file, set WAL mode (persists):
// PRAGMA journal_mode=WAL;
// PRAGMA busy_timeout=5000;
```

**WAL mode allows:**
- Multiple concurrent readers (publish pipeline reading while admin browses)
- One writer at a time, with readers not blocked by writes
- Readers not blocked by other readers

**Remaining constraint:** Two simultaneous writes (e.g., two admin users editing the same site's content at the same time) will serialize. For this system's scale, that's acceptable.

### 7.2 Core Database (Shared)

The only shared database. Located at `/var/wpm/core.db`.

```csharp
// CoreDbContext.cs — the ONLY shared DbContext
public class CoreDbContext : DbContext
{
    public DbSet<Site> Sites => Set<Site>();
    public DbSet<SiteDomain> SiteDomains => Set<SiteDomain>();
    public DbSet<User> Users => Set<User>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=/var/wpm/core.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SiteDomain>(e =>
        {
            e.HasIndex(d => d.DomainName).IsUnique();
            e.HasOne<Site>().WithMany().HasForeignKey(d => d.SiteId);
        });
    }
}
```

```csharp
/// <summary>
/// A registered site. The SiteFolder is the subfolder name under /var/wpm/data/.
/// The platform discovers which domains are enabled by scanning which .db files
/// exist in that folder — no need for an EnabledDomains list.
/// </summary>
public class Site
{
    public int Id { get; set; }
    public string Name { get; set; } = "";           // "Frog's Folly"
    public string SiteFolder { get; set; } = "";     // "frogsfolly.com"
    public string PrimaryDomain { get; set; } = "";  // "frogsfolly.com"
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class SiteDomain
{
    public int Id { get; set; }
    public int SiteId { get; set; }
    public string DomainName { get; set; } = "";     // "frogsfolly.com"
    public bool IsPrimary { get; set; }
}

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public UserRole Role { get; set; } = UserRole.Anonymous;
    public List<int> AuthorizedSiteIds { get; set; } = []; // which sites this user can manage
}

public enum UserRole
{
    Anonymous = 4,
    User = 3,
    Editor = 2,
    Admin = 1           // Matches current system's UserGroupID values
}
```

### 7.3 Domain-Owned DbContexts

Each content domain creates its own `DbContext` that connects to a specific `.db` file in the site folder. **No TenantId columns, no global query filters.**

```csharp
// In WPM.Domain.CMS:
public class CmsDbContext : DbContext
{
    private readonly string _dbPath;

    public CmsDbContext(string dbPath) => _dbPath = dbPath;

    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<Part> Parts => Set<Part>();
    public DbSet<Parameter> Parameters => Set<Parameter>();
    public DbSet<LocationGroup> LocationGroups => Set<LocationGroup>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={_dbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Location>(e =>
        {
            e.HasMany(l => l.Articles).WithOne().HasForeignKey(a => a.LocationId);
            e.HasOne<Location>().WithMany().HasForeignKey(l => l.ParentLocationId);
        });
        // ... remaining entity configuration
    }
}

// In WPM.Domain.Minerals:
public class MineralsDbContext : DbContext
{
    private readonly string _dbPath;

    public MineralsDbContext(string dbPath) => _dbPath = dbPath;

    public DbSet<Specimen> Specimens => Set<Specimen>();
    public DbSet<Mineral> Minerals => Set<Mineral>();
    public DbSet<SpecimenImage> SpecimenImages => Set<SpecimenImage>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={_dbPath}");
}

// In WPM.Domain.Recipes:
public class RecipesDbContext : DbContext
{
    private readonly string _dbPath;

    public RecipesDbContext(string dbPath) => _dbPath = dbPath;

    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<RecipeCategory> Categories => Set<RecipeCategory>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={_dbPath}");
}
```

### 7.4 Site Configuration (`site.json`)

Each site folder contains a `site.json` file with site-level configuration:

```json
{
  "theme": "default",
  "homePageSlug": "home",
  "siteName": "JM Shaw Minerals",
  "tagline": "A Collection of Fine Minerals",
  "contactEmail": "info@jmshawminerals.com",
  "analytics": { "googleId": "G-XXXXX" },
  "navigation": { "maxDepth": 3, "showBreadcrumbs": true },
  "publishing": { "generateRss": true, "generateSitemap": true }
}
```

### 7.5 Domain Enablement Detection

The platform doesn't need a config field to know which domains a site uses — it scans the folder:

```csharp
public class SiteDomainDetector
{
    private readonly IReadOnlyList<IContentDomain> _domains;

    /// Returns which content domains are enabled for a given site folder.
    public IReadOnlyList<IContentDomain> GetEnabledDomains(string siteFolder)
    {
        return _domains
            .Where(d => File.Exists(Path.Combine(siteFolder, d.DatabaseFileName)))
            .ToList();
    }
}
```

### 7.6 Mapping from Legacy Tables

> **Critical Finding (Feb 2026 Review):** The legacy database table is called `Page`, not
> `Location`. The `Location` class in the VB.NET code is an **in-memory entity** populated from
> `Page` + `Article` data via `ApplicationDAL.GetSiteMap()`. The documentation previously
> confused the in-memory class with the database table. Additionally, the `Page` table has a
> direct `CompanyID` column used for filtering — this is how data is partitioned when multiple
> companies share one `.mdb` file.

#### CMS Tables (from .mdb files via OLE DB)

| Legacy DB Table | Has CompanyID? | New Location | Notes |
|-----------------|---------------|-------------|-------|
| **Company** | N/A (is the company) | `core.db` → `Sites` + `SiteDomains` | One Site per CompanyID; SiteURL → SiteDomain |
| **Page** | **Yes** | `{site}/cms.db` → `Locations` | ParentLocationID self-ref FK; filtered by `Page.CompanyID` |
| **Article** | **Yes** | `{site}/cms.db` → `Articles` | ArticlePageID → FK to Locations; filtered by `Article.CompanyID` |
| **Part** | Yes | `{site}/cms.db` → `Parts` | Reusable content blocks |
| **Parameter** | Yes | `{site}/cms.db` → `Parameters` | Key-value config per site |
| **Image** | **Yes** | `{site}/cms.db` → `Images` | Image metadata; filtered by `Image.CompanyID` |
| **PageImage** | No (junction) | `{site}/cms.db` → `LocationImages` | Junction: PageID + ImageID + Position |
| **PageAlias** | **Yes** | `{site}/cms.db` → `LocationAliases` | URL redirect mappings; critical for SEO migration |
| **LocationGroup** | Yes | `{site}/cms.db` → `LocationGroups` | |
| **SiteCategoryType** | Yes | `{site}/cms.db` → `Categories` | Site category taxonomy |

#### Mineral Tables (from Azure SQL — NOT from .mdb files)

> **Note:** The `wpmMineralCollection` project originally connected to Azure SQL
> (`markhazleton2.database.windows.net`), which is **no longer accessible**. SQL scripts to
> recreate the database exist. The `MineralCollection.mdb` files in App_Data contain CMS page
> data for the mineral site; the specimen/mineral data comes from the SQL scripts.

| Legacy DB Table | New Location | Notes |
|-----------------|-------------|-------|
| **Collection** | `{site}/minerals.db` → `Collections` | Collection groupings |
| **CollectionItem** | `{site}/minerals.db` → `Specimens` | Core specimen data: dimensions, price, mine, purchase info |
| **Mineral** | `{site}/minerals.db` → `Minerals` | Mineral type definitions with Wikipedia links |
| **CollectionItemMineral** | `{site}/minerals.db` → `SpecimenMinerals` | Many-to-many: specimen ↔ mineral |
| **CollectionItemImage** | `{site}/minerals.db` → `SpecimenImages` | Specimen photos with display order |
| **Company** | `{site}/minerals.db` → `Dealers` | Dealers/sellers (different from CMS Company table) |
| **LocationCity** | `{site}/minerals.db` → `Cities` | City + lat/long + county |
| **LocationState** | `{site}/minerals.db` → `States` | State/province |
| **LocationCountry** | `{site}/minerals.db` → `Countries` | Country |

#### Recipe Tables (from AWS RDS — NOT from .mdb files)

> **Note:** The `WPMRecipe` project originally connected to AWS RDS
> (`controlorigins1.cnggm5xnvplw.us-west-2.rds.amazonaws.com`), which is **no longer
> accessible**. SQL scripts to recreate the database exist.

| Legacy DB Table | New Location | Notes |
|-----------------|-------------|-------|
| **Recipe** | `{site}/recipes.db` → `Recipes` | Includes ratings, view counts, comments count |
| **RecipeImage** | `{site}/recipes.db` → `RecipeImages` | Recipe photos with display order |
| **RecipeComment** | `{site}/recipes.db` → `RecipeComments` | User comments with author info |
| **RecipeCategory** | `{site}/recipes.db` → `RecipeCategories` | Category taxonomy |

#### Data Partitioning During Migration

When a single .mdb file contains multiple companies (e.g., `FrogsFollyKids.mdb` → 7 subdomains):

```
FrogsFollyKids.mdb
├── Company table: rows with CompanyID 4, 5, 6, 7, 8, 9, 10
├── Page table: rows with CompanyID 4, 5, 6, 7, 8, 9, 10
├── Article table: rows with CompanyID 4, 5, 6, 7, 8, 9, 10
└── Image table: rows with CompanyID 4, 5, 6, 7, 8, 9, 10

Migration splits into:
├── lauren.frogsfolly.com/cms.db  ← WHERE CompanyID = 4
├── sarah.frogsfolly.com/cms.db   ← WHERE CompanyID = 5
├── jordan.frogsfolly.com/cms.db  ← WHERE CompanyID = 6
└── ... (one folder per CompanyID)
```

---

## 8. API Layer

### 8.1 Startup: Domain Discovery + Schema Validation

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Auto-discover all IContentDomain implementations
var domainTypes = AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(a => a.GetTypes())
    .Where(t => typeof(IContentDomain).IsAssignableFrom(t)
             && !t.IsInterface && !t.IsAbstract);

var domains = domainTypes
    .Select(t => (IContentDomain)Activator.CreateInstance(t)!)
    .ToList();

// Register core services
builder.Services.AddSingleton<IReadOnlyList<IContentDomain>>(domains);
builder.Services.AddDbContext<CoreDbContext>();              // Shared: sites, users, auth
builder.Services.AddScoped<ISiteService, SiteService>();
builder.Services.AddScoped<SiteDomainDetector>();
builder.Services.AddScoped<PublishingService>();
builder.Services.AddSingleton<ITemplateEngine, ScribanTemplateEngine>();

// Let each domain register its own services (NOT DbContexts — those are per-request)
foreach (var domain in domains)
    domain.ConfigureServices(builder.Services);

builder.Services.AddAuthentication().AddJwtBearer(/* ... */);
builder.Services.AddAuthorization();

var app = builder.Build();

// === Startup: Validate all site databases against schema contracts ===
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var dataRoot = "/var/wpm/data";

    foreach (var siteFolder in Directory.GetDirectories(dataRoot))
    {
        var siteName = Path.GetFileName(siteFolder);
        foreach (var domain in domains)
        {
            var dbPath = Path.Combine(siteFolder, domain.DatabaseFileName);
            if (!File.Exists(dbPath)) continue;

            var contract = domain.GetSchemaContract();
            var violations = await contract.ValidateAsync(dbPath);

            if (violations.Any())
                logger.LogError("Site {Site}/{Domain}: {Count} schema violations found",
                    siteName, domain.DomainId, violations.Count);
            else
                logger.LogInformation("Site {Site}/{Domain}: schema valid ✓",
                    siteName, domain.DomainId);
        }
    }
}

// Middleware
app.UseMiddleware<SiteMiddleware>();   // Host header → SiteContext
app.UseAuthentication();
app.UseAuthorization();

// Core endpoints
app.MapSiteEndpoints();
app.MapAuthEndpoints();
app.MapPublishEndpoints();

// Let each domain register its endpoints
foreach (var domain in domains)
    domain.MapEndpoints(app);

app.Run();
```

### 8.2 Site Middleware (replaces TenantMiddleware)

The middleware resolves the incoming `Host` header to a site folder path and sets the `SiteContext` for the request. Domain endpoints then open their own DbContext using the site folder path.

```csharp
public class SiteMiddleware
{
    private readonly RequestDelegate _next;

    public async Task InvokeAsync(HttpContext context, ISiteService siteService)
    {
        var host = context.Request.Host.Host;
        var domain = host.Replace("www.", "").ToLowerInvariant();

        var site = await siteService.ResolveByDomainAsync(domain);
        if (site != null)
        {
            var dataFolder = Path.Combine("/var/wpm/data", site.SiteFolder);
            var siteConfig = SiteConfig.Load(Path.Combine(dataFolder, "site.json"));

            var siteContext = new SiteContext(
                SiteDomain: site.PrimaryDomain,
                DataFolder: dataFolder,
                OutputFolder: Path.Combine("/var/wpm/sites", site.PrimaryDomain),
                MediaFolder: Path.Combine(dataFolder, "media"),
                Config: siteConfig,
                Services: context.RequestServices
            );

            // Make SiteContext available to downstream handlers
            context.Items["SiteContext"] = siteContext;
        }

        await _next(context);
    }
}

// Extension to easily get SiteContext from HttpContext
public static class SiteContextExtensions
{
    public static SiteContext GetSiteContext(this HttpContext context)
        => (SiteContext)context.Items["SiteContext"]!;

    /// Open a domain-specific DbContext for the current site.
    /// Example: var db = context.GetDomainDb<CmsDbContext>("cms.db");
    public static T GetDomainDb<T>(this HttpContext context, string dbFileName)
        where T : DbContext
    {
        var site = context.GetSiteContext();
        var dbPath = Path.Combine(site.DataFolder, dbFileName);
        return (T)Activator.CreateInstance(typeof(T), dbPath)!;
    }
}
```

### 8.3 Domain Endpoint Example (Using Site-Scoped DB)

```csharp
// In WPM.Domain.CMS/Endpoints/LocationEndpoints.cs
public static class LocationEndpoints
{
    public static void Map(IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/cms").RequireAuthorization();

        group.MapGet("/locations", async (HttpContext http) =>
        {
            using var db = http.GetDomainDb<CmsDbContext>("cms.db");
            var locations = await db.Locations
                .OrderBy(l => l.Level).ThenBy(l => l.SortOrder)
                .ToListAsync();
            return Results.Ok(locations);
        });

        group.MapGet("/locations/{id:int}", async (int id, HttpContext http) =>
        {
            using var db = http.GetDomainDb<CmsDbContext>("cms.db");
            var location = await db.Locations
                .Include(l => l.Articles)
                .FirstOrDefaultAsync(l => l.Id == id);
            return location is null ? Results.NotFound() : Results.Ok(location);
        });

        group.MapPut("/locations/{id:int}", async (int id, Location update, HttpContext http) =>
        {
            using var db = http.GetDomainDb<CmsDbContext>("cms.db");
            var location = await db.Locations.FindAsync(id);
            if (location is null) return Results.NotFound();

            location.Name = update.Name;
            location.Description = update.Description;
            location.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();

            return Results.Ok(location);
        });
    }
}
```

### 8.4 API Endpoint Examples

```
Core:
  POST /api/auth/login                           → JWT token
  GET  /api/sites                                → list sites (super-admin)
  POST /api/publish/{siteDomain}                 → trigger full publish
  POST /api/publish/{siteDomain}/{domainId}/{itemId}  → incremental publish

CMS Domain (registered by WPM.Domain.CMS):
  GET    /api/cms/locations                      → location tree (site-scoped via SiteMiddleware)
  GET    /api/cms/locations/{id}
  POST   /api/cms/locations
  PUT    /api/cms/locations/{id}
  DELETE /api/cms/locations/{id}
  GET    /api/cms/articles
  POST   /api/cms/articles
  ...

Minerals Domain (registered by WPM.Domain.Minerals):
  GET    /api/minerals/specimens                 → specimens (from minerals.db in site folder)
  GET    /api/minerals/specimens/{id}
  POST   /api/minerals/specimens
  PUT    /api/minerals/specimens/{id}
  DELETE /api/minerals/specimens/{id}
  POST   /api/minerals/specimens/{id}/images
  ...

Recipes Domain (registered by WPM.Domain.Recipes):
  GET    /api/recipes                            → recipes (from recipes.db in site folder)
  POST   /api/recipes
  ...
```

---

## 9. Publishing Pipeline

### 9.1 How It Works

Publishing transforms database content into static HTML files on disk. Caddy serves them immediately — no restart needed.

```
Admin clicks "Publish"
       │
       ▼
POST /api/publish/{siteDomain}
       │
       ▼
PublishingService.PublishSiteAsync()
       │
       ├── Resolves site folder: /var/wpm/data/{siteDomain}/
       │
       ├── Scans folder for .db files → determines enabled domains
       │
       ├── Builds SiteContext (folder paths, config, services)
       │
       ├── For each enabled domain's IContentPublisher:
       │   │
       │   ├── CmsPublisher.PublishAsync()
       │   │   → Opens cms.db from site folder
       │   │   → Generates: index.html, about/index.html, blog/post-1/index.html
       │   │   → Generates: sitemap.xml, rss.xml, 404.html
       │   │
       │   ├── MineralsPublisher.PublishAsync()  (if minerals.db exists)
       │   │   → Opens minerals.db from site folder
       │   │   → Generates: collection/index.html, collection/quartz/index.html
       │   │   → Generates: collection/data.json (for client-side filtering)
       │   │
       │   └── RecipesPublisher.PublishAsync()  (if recipes.db exists)
       │       → Opens recipes.db from site folder
       │       → Generates: recipes/index.html, recipes/chocolate-cake/index.html
       │
       ├── Writes all files to /var/wpm/sites/{siteDomain}/
       │
       └── Returns publish report (files written, errors, duration)
```

### 9.2 Publishing Service (Core)

```csharp
public class PublishingService
{
    private readonly IReadOnlyList<IContentDomain> _domains;
    private readonly IEnumerable<IContentPublisher> _publishers;
    private readonly ISiteService _siteService;
    private readonly SiteDomainDetector _detector;
    private readonly ILogger<PublishingService> _logger;

    public async Task<PublishReport> PublishSiteAsync(string siteDomain, CancellationToken ct)
    {
        var site = await _siteService.GetByDomainAsync(siteDomain);
        var dataFolder = Path.Combine("/var/wpm/data", site.SiteFolder);
        var outputFolder = Path.Combine("/var/wpm/sites", site.PrimaryDomain);
        var siteConfig = SiteConfig.Load(Path.Combine(dataFolder, "site.json"));

        var context = new SiteContext(
            SiteDomain: site.PrimaryDomain,
            DataFolder: dataFolder,
            OutputFolder: outputFolder,
            MediaFolder: Path.Combine(dataFolder, "media"),
            Config: siteConfig,
            Services: _services
        );

        // Detect which domains are enabled (which .db files exist)
        var enabledDomains = _detector.GetEnabledDomains(dataFolder);

        var report = new PublishReport { SiteDomain = siteDomain, StartedAt = DateTime.UtcNow };

        foreach (var publisher in _publishers)
        {
            if (!enabledDomains.Any(d => d.DomainId == publisher.DomainId))
                continue;

            var files = await publisher.PublishAsync(context, ct);

            foreach (var file in files)
            {
                var fullPath = Path.Combine(outputFolder, file.RelativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
                await File.WriteAllTextAsync(fullPath, file.Content, ct);
                report.FilesWritten++;
            }
        }

        report.CompletedAt = DateTime.UtcNow;
        _logger.LogInformation("Published {Site}: {Files} files in {Duration}ms",
            siteDomain, report.FilesWritten,
            (report.CompletedAt - report.StartedAt).TotalMilliseconds);
        return report;
    }
}
```

### 9.3 Template Engine

Using **Scriban** (lightweight, Liquid-compatible, .NET native):

```csharp
public class ScribanTemplateEngine : ITemplateEngine
{
    private readonly ConcurrentDictionary<string, Template> _cache = new();

    public async Task<string> RenderAsync(string templateName, object model)
    {
        var template = _cache.GetOrAdd(templateName, name =>
        {
            var source = File.ReadAllText($"Templates/{name}.scriban");
            return Template.Parse(source);
        });

        var context = new TemplateContext();
        var scriptObject = new ScriptObject();
        scriptObject.Import(model);
        context.PushGlobal(scriptObject);

        return await template.RenderAsync(context);
    }
}
```

### 9.4 Example: CMS Publisher

```csharp
public class CmsPublisher : IContentPublisher
{
    public string DomainId => "cms";

    public async Task<IReadOnlyList<PublishableFile>> PublishAsync(
        SiteContext context, CancellationToken ct)
    {
        var files = new List<PublishableFile>();

        // Open THIS SITE's cms.db (not a shared DB)
        var dbPath = Path.Combine(context.DataFolder, "cms.db");
        using var db = new CmsDbContext(dbPath);
        var templates = context.Services.GetRequiredService<ITemplateEngine>();

        // Load all locations for this site (no TenantId filter needed!)
        var locations = await db.Locations
            .Where(l => l.IsActive)
            .Include(l => l.Articles.Where(a => a.IsActive))
            .OrderBy(l => l.Level).ThenBy(l => l.SortOrder)
            .ToListAsync(ct);

        // Build navigation tree
        var navTree = BuildTree(locations);

        // Generate page for each location
        foreach (var loc in locations)
        {
            var slug = loc.BreadcrumbUrl?.Trim('/') ?? loc.Slug ?? loc.Id.ToString();
            files.Add(new PublishableFile(
                $"{slug}/index.html",
                await templates.RenderAsync("page", new
                {
                    location = loc,
                    navigation = navTree,
                    site = context.Config
                }),
                "text/html"
            ));
        }

        // Generate sitemap.xml
        files.Add(new PublishableFile(
            "sitemap.xml",
            await templates.RenderAsync("sitemap", new { locations, domain = context.SiteDomain }),
            "application/xml"
        ));

        // Generate 404 page
        files.Add(new PublishableFile(
            "404.html",
            await templates.RenderAsync("404", new { navigation = navTree, site = context.Config }),
            "text/html"
        ));

        return files;
    }
}
```

---

## 10. Admin UI

### 10.0 Phasing Strategy: Server-Rendered First, React Later

> **Recommendation (Feb 2026 Review):** A full React 19 SPA with Vite + TypeScript + module
> system is a significant build that can easily consume 4–8 weeks of effort. For a solo or
> small team, this delays getting sites live.
>
> **Practical approach:**
> 1. **Phase 1 (MVP):** Build admin as Razor Pages in the same ASP.NET Core app. Server-rendered
>    forms for editing Locations, Articles, and triggering publish. Gets you editing content in
>    days instead of weeks.
> 2. **Phase 2 (Enhancement):** Once all sites are published and stable, build the React SPA
>    as a progressive upgrade. The API endpoints built for the Razor Pages admin work unchanged
>    for the React admin.
>
> **If React is chosen from the start**, keep the scope minimal: site picker, location tree
> editor, article editor, publish button. Skip minerals/recipes admin modules until those
> domains are proven.

### 10.1 Architecture (Target: React SPA)

The React admin is a **shell + modules** pattern:

```
admin/src/
├── shell/
│   ├── App.tsx              # Root component: router, auth context, site context
│   ├── Layout.tsx           # Sidebar nav + content area
│   ├── SitePicker.tsx       # Dropdown to switch site context
│   ├── AuthProvider.tsx     # JWT auth state
│   └── DomainRouter.tsx     # Dynamically loads modules based on site's enabled domains
│
├── modules/
│   ├── cms/
│   │   ├── LocationTreeEditor.tsx
│   │   ├── ArticleEditor.tsx
│   │   ├── PartsEditor.tsx
│   │   └── index.ts         # Exports routes + nav items
│   │
│   ├── minerals/
│   │   ├── SpecimenList.tsx
│   │   ├── SpecimenEditor.tsx
│   │   ├── ImageUpload.tsx
│   │   └── index.ts
│   │
│   └── recipes/
│       ├── RecipeList.tsx
│       ├── RecipeEditor.tsx
│       └── index.ts
│
└── shared/
    ├── RichTextEditor.tsx    # TipTap or Lexical
    ├── DataTable.tsx
    ├── ImageManager.tsx
    └── PublishButton.tsx     # Triggers POST /api/publish/{siteDomain}
```

### 10.2 Module Registration (Admin Side)

```typescript
// admin/src/modules/index.ts
import { cmsModule } from './cms';
import { mineralsModule } from './minerals';
import { recipesModule } from './recipes';

export interface AdminModule {
  domainId: string;
  displayName: string;
  routes: RouteObject[];
  navItems: NavItem[];
}

// All available modules — the shell filters by which .db files exist for the site
export const allModules: AdminModule[] = [
  cmsModule,
  mineralsModule,
  recipesModule,
];
```

### 10.3 How the Shell Loads Modules

The admin shell calls `GET /api/sites/{domain}/domains` to discover which content domains are enabled (i.e., which `.db` files exist in the site folder). It only shows modules for enabled domains.

```typescript
// admin/src/shell/DomainRouter.tsx
function DomainRouter() {
  const { site } = useSite();  // current site context
  // site.enabledDomains comes from API: ["cms", "minerals"]
  const enabledModules = allModules.filter(m =>
    site.enabledDomains.includes(m.domainId)
  );

  return (
    <>
      <Sidebar navItems={enabledModules.flatMap(m => m.navItems)} />
      <Routes>
        {enabledModules.flatMap(m => m.routes)}
      </Routes>
    </>
  );
}
```

---

## 11. Hosting & Infrastructure

### 11.1 VM Specification

| Spec | Value |
|------|-------|
| **Provider** | Azure |
| **SKU** | B1s (1 vCPU, 1 GB RAM) or B1ms (1 vCPU, 2 GB RAM) |
| **OS** | Ubuntu 24.04 LTS |
| **Disk** | 30 GB managed SSD (P4) |
| **Estimated Cost** | ~$7–14/month |

### 11.2 Software on VM

| Software | Purpose | Install |
|----------|---------|---------|
| Caddy 2 | Web server, reverse proxy, auto-SSL | `apt install caddy` |
| .NET 9 Runtime | Run ASP.NET Core API | `apt install dotnet-runtime-9.0` |
| SQLite 3 | Database (used via EF Core, CLI for backups) | Pre-installed on Ubuntu |

### 11.3 Service Configuration

**systemd unit file** (`/etc/systemd/system/wpm-api.service`):

```ini
[Unit]
Description=WPM API
After=network.target

[Service]
ExecStart=/usr/bin/dotnet /opt/wpm/api/WPM.Api.dll
WorkingDirectory=/opt/wpm/api
Restart=always
RestartSec=5
User=wpm
Environment=ASPNETCORE_URLS=http://localhost:5000
Environment=ASPNETCORE_ENVIRONMENT=Production

[Install]
WantedBy=multi-user.target
```

### 11.4 Directory Layout on VM

```
/opt/wpm/api/                              # Published ASP.NET Core API binary

/var/wpm/
├── core.db                                # Shared: sites, users, auth
│
├── data/                                  # Source data (per-site folders)
│   ├── frogsfolly.com/
│   │   ├── cms.db                         # CMS content for this site
│   │   ├── site.json                      # Site config (theme, etc.)
│   │   └── media/                         # Images for this site
│   ├── jmshawminerals.com/
│   │   ├── cms.db
│   │   ├── minerals.db                    # Mineral collection DB
│   │   ├── site.json
│   │   └── media/
│   └── ...                                # One folder per site
│
├── sites/                                 # Published static output (Caddy serves this)
│   ├── frogsfolly.com/                    # HTML files
│   ├── jmshawminerals.com/
│   ├── admin/                             # React admin SPA
│   └── ...
│
└── backups/                               # Nightly backups (zip per site folder)
    ├── 2026-02-01/
    │   ├── core.db.bak
    │   ├── frogsfolly.com.zip
    │   ├── jmshawminerals.com.zip
    │   └── ...
    └── ...
```

**Backup strategy:** Each site folder is zipped independently. `core.db` is backed up separately. Daily rotation with 7-day retention. Can push to Azure Blob Storage for off-site backup.

### 11.5 Cost Breakdown

| Item | Monthly Cost |
|------|-------------|
| Azure Linux VM (B1s) | ~$7 |
| Managed disk (30 GB SSD) | ~$2 |
| SSL certificates (Let's Encrypt via Caddy) | $0 |
| GitHub Actions (free tier) | $0 |
| Azure DNS (optional) | ~$1 |
| **Total** | **~$10/month** |

---

## 12. Multi-Domain Routing (Caddy)

### 12.1 How It Works

One Caddyfile handles all 36+ domains. Each domain block maps to a folder on disk. Caddy automatically obtains and renews SSL certificates for every domain.

### 12.2 Caddyfile Structure

```
# === Site Domains ===
# (one block per domain — auto-generated from Tenants table)

frogsfolly.com {
    root * /var/wpm/sites/frogsfolly.com
    file_server
    handle /api/* {
        reverse_proxy localhost:5000
    }
    handle_errors {
        @404 expression {http.error.status_code} == 404
        rewrite @404 /404.html
        file_server
    }
}

jmshawminerals.com {
    root * /var/wpm/sites/jmshawminerals.com
    file_server
    handle /api/* {
        reverse_proxy localhost:5000
    }
    handle_errors {
        @404 expression {http.error.status_code} == 404
        rewrite @404 /404.html
        file_server
    }
}

# ... one block per active domain

# === WWW Redirects ===
# (301 redirect www → non-www for each domain)

www.frogsfolly.com {
    redir https://frogsfolly.com{uri} 301
}

www.jmshawminerals.com {
    redir https://jmshawminerals.com{uri} 301
}

# ... one redirect per domain

# === Admin ===

admin.wpm.com {
    root * /var/wpm/sites/admin
    try_files {path} /index.html    # SPA fallback
    file_server
    handle /api/* {
        reverse_proxy localhost:5000
    }
}
```

### 12.3 Generating the Caddyfile

The Caddyfile can be auto-generated from the database during deployment:

```csharp
// Tool: GenerateCaddyfile.cs
var db = new CoreDbContext();
var sites = db.Sites.Include(s => s.Domains).Where(s => s.IsActive).ToList();

foreach (var site in sites)
{
    foreach (var domain in site.Domains)
    {
        sb.AppendLine($"{domain.DomainName} {{");
        sb.AppendLine($"    root * /var/wpm/sites/{site.PrimaryDomain}");
        sb.AppendLine($"    file_server");
        // Serve media from the site's data folder
        sb.AppendLine($"    handle /media/* {{");
        sb.AppendLine($"        root * /var/wpm/data/{site.SiteFolder}");
        sb.AppendLine($"        file_server");
        sb.AppendLine($"    }}");
        sb.AppendLine($"    handle /api/* {{");
        sb.AppendLine($"        reverse_proxy localhost:5000");
        sb.AppendLine($"    }}");
        sb.AppendLine($"}}");
        // www redirect
        sb.AppendLine($"www.{domain.DomainName} {{");
        sb.AppendLine($"    redir https://{domain.DomainName}{{uri}} 301");
        sb.AppendLine($"}}");
    }
}
```

After generating, run `systemctl reload caddy` — zero-downtime config reload.

---

## 13. Migration from Legacy System

### 13.0 Critical Findings: Data Lives in Three Places

> **Feb 2026 Review:** The legacy data is **not** all in .mdb files. Codebase analysis revealed
> three distinct data sources that must each be handled differently.

| Data Source | Technology | Data | Migration Method |
|------------|-----------|------|-----------------|
| **16 .mdb files** (App_Data/) | MS Access via OLE DB | CMS content: Company, Page, Article, Part, Parameter, Image, PageImage, PageAlias | Read directly via `System.Data.OleDb` (**Windows-only**) |
| **SQL scripts** (provided) | SQL DDL + data | Mineral collection: Collection, CollectionItem, Mineral, CollectionItemImage, location geography | Run SQL scripts to create local SQLite DB, then migrate |
| **SQL scripts** (provided) | SQL DDL + data | Recipes: Recipe, RecipeImage, RecipeComment, RecipeCategory | Run SQL scripts to create local SQLite DB, then migrate |
| **Web server filesystem** | Files on disk | Images referenced by `Image.ImageFileName` and `Company.GalleryFolder` paths | Copy from legacy IIS server to per-site `/media/` folders |

> **Resolved (Feb 2026):** The original Azure SQL (minerals) and AWS RDS (recipes) databases
> are **no longer accessible**. However, the SQL scripts to recreate these databases exist.
> The migration strategy is: run the SQL scripts locally (into a temp SQL Server LocalDB or
> directly into SQLite), then migrate from there into per-site `.db` files.
>
> **Images** are confirmed to be on the **filesystem of the legacy IIS web server**. They are
> not embedded in databases or stored in cloud blob storage. Migration requires copying files
> from the legacy server's filesystem, organized by each site's `GalleryFolder` path.

**Why not use the existing APIs?** The legacy `.ashx` APIs (`CompanyApi.ashx`, `LocationApi.ashx`, `UserApi.ashx`) are thin runtime endpoints. The Location API is a generic geospatial service (lat/long search), not the CMS page hierarchy. The Company API has pagination but doesn't return nested data (locations, articles, parts). **Direct database access is the only viable migration path.**

### 13.1 Pre-Migration: Schema Discovery (Phase 0)

Before writing any migration code, **dump the actual schema from every data source.** The in-memory VB.NET classes don't match the database tables 1:1 (e.g., `Location.vb` class ≠ `Location` database table — the DB table is called `Page`).

**Step 1: Dump .mdb schemas**

```csharp
// WPM.Migration/SchemaDiscovery.cs — run first, before any data migration
foreach (var mdbFile in Directory.GetFiles(appDataPath, "*.mdb"))
{
    using var conn = new OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={mdbFile}");
    conn.Open();

    // Get all tables
    var tables = conn.GetSchema("Tables")
        .AsEnumerable()
        .Where(r => r["TABLE_TYPE"].ToString() == "TABLE")
        .Select(r => r["TABLE_NAME"].ToString());

    foreach (var table in tables)
    {
        // Get columns for each table
        var columns = conn.GetSchema("Columns", new[] { null, null, table });
        // Output: table name, column name, data type, nullable
    }
}
```

**Expected output:** A CSV/JSON report showing every table and column in every .mdb file. Compare across databases to confirm which share the same schema and which differ.

**Step 2: Verify Azure SQL mineral schema**

Connect to `markhazleton2.database.windows.net` and run `SELECT TABLE_NAME, COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS`. Compare against the LINQ to SQL designer file in `wpmMineralCollection/DataClasses1.designer.vb`.

**Step 3: Verify AWS RDS recipe schema**

Connect to `controlorigins1.cnggm5xnvplw.us-west-2.rds.amazonaws.com` and run the same. Compare against `WPMRecipe/RecipeLibrary.designer.vb`.

**Step 4: Map CompanyID → data volume**

For each .mdb file, run:
```sql
SELECT CompanyID, COUNT(*) as PageCount FROM Page GROUP BY CompanyID;
SELECT CompanyID, COUNT(*) as ArticleCount FROM Article GROUP BY CompanyID;
SELECT CompanyID, COUNT(*) as ImageCount FROM Image GROUP BY CompanyID;
```

This tells you exactly how much data belongs to each site and validates the XML config mappings.

### 13.2 Data Migration Tool

**Platform:** .NET 9 console app targeting **Windows** (OLE DB provider for Access is Windows-only).

**Run locally**, then copy resulting SQLite files to the Linux VM.

```
WPM.Migration workflow:

┌─────────────────────────────────────────────────────────┐
│  Phase A: CMS Content (from 16 .mdb files)              │
│                                                          │
│  For each .mdb file:                                     │
│  1. Open OleDbConnection                                 │
│  2. Query Company table → get list of CompanyIDs         │
│  3. For each CompanyID:                                  │
│     a. Look up domain from XML config files              │
│        (App_Data/Sites/{domain}.xml → CompanyID match)   │
│     b. Create site folder: /output/{domain}/             │
│     c. Create cms.db via EF Core (CmsDbContext)          │
│     d. Migrate Page rows WHERE CompanyID = X             │
│        → INSERT into Locations table                     │
│        → Preserve ParentLocationID self-reference        │
│        → Build old PageID → new LocationID mapping       │
│     e. Migrate Article rows WHERE CompanyID = X          │
│        → INSERT into Articles, remap ArticlePageID       │
│     f. Migrate Part rows WHERE CompanyID = X             │
│     g. Migrate Parameter rows WHERE CompanyID = X        │
│     h. Migrate Image rows WHERE CompanyID = X            │
│     i. Migrate PageImage junction rows                   │
│        (WHERE PageID in migrated pages)                  │
│     j. Migrate PageAlias rows WHERE CompanyID = X        │
│        → These become URL redirect entries               │
│     k. Generate site.json from Company row               │
│        (SiteTemplate, FromEmail, SMTP, etc.)             │
│  4. Insert Site + SiteDomain rows into core.db           │
│                                                          │
│  Phase B: Mineral Collection (from SQL scripts)          │
│                                                          │
│  1. Run provided SQL scripts into temp SQL Server        │
│     LocalDB (or adapt scripts for direct SQLite import)  │
│  2. Create jmshawminerals.com/minerals.db               │
│  3. Migrate: Collection, CollectionItem, Mineral,        │
│     CollectionItemMineral, CollectionItemImage,          │
│     Company (as Dealers), LocationCity/State/Country     │
│  4. Copy specimen images from legacy server filesystem   │
│     → media/specimens/                                   │
│                                                          │
│  Phase C: Recipes (from SQL scripts)                     │
│                                                          │
│  1. Run provided SQL scripts into temp SQL Server        │
│     LocalDB (or adapt scripts for direct SQLite import)  │
│  2. Create {recipe-site}/recipes.db                      │
│  3. Migrate: Recipe, RecipeImage, RecipeComment,         │
│     RecipeCategory                                       │
│  4. Copy recipe images from legacy server filesystem     │
│     → media/recipes/                                     │
│                                                          │
│  Phase D: Validation                                     │
│                                                          │
│  1. For each site folder:                                │
│     - Count rows in SQLite vs source (must match)        │
│     - Verify FK integrity (no orphan articles)           │
│     - Verify PageAlias URLs are preserved                │
│     - Spot-check HTML content (not corrupted)            │
│  2. Generate migration report (CSV)                      │
│     - Site, table, source rows, migrated rows, skipped   │
└─────────────────────────────────────────────────────────┘
```

**Key implementation detail — ID remapping:**

The migration must handle the case where a shared .mdb has multiple companies. When splitting `FrogsFollyKids.mdb` (CompanyIDs 4–10) into 7 separate SQLite files, the auto-increment IDs in each new `cms.db` will differ from the original PageIDs. The migration tool must:

1. Build a mapping: `old PageID → new LocationID` per site
2. Update all `ParentLocationID` references using this mapping
3. Update all `ArticlePageID` references using this mapping
4. Update all `PageImage.PageID` references using this mapping
5. Update all internal links in HTML content (`Default.aspx?c={old_id}`)

Alternatively, **preserve original IDs** by inserting with explicit IDs (disable auto-increment during migration). This is simpler and avoids remapping.

### 13.3 URL Redirect Map

**Critical for SEO.** Generate 301 redirects for every legacy URL.

Legacy URL patterns:
- `Default.aspx?c={PageID}` → `/{slug}/`
- `Default.aspx?c={PageID}&a={ArticleID}` → `/{slug}/{article-slug}/`
- `/{breadcrumb}/page.aspx` → `/{breadcrumb}/`
- PageAlias patterns (wildcard + literal matches from `PageAlias` table)

**Source data:** The `PageAlias` table already contains URL redirect mappings per company. Migrate these as-is, then add the `Default.aspx?c=X` → slug mappings on top.

Approach:
1. During CMS migration, for each Page/Article, record: `{old URL} → {new slug}`
2. Export PageAlias table entries per site
3. Generate Caddy redirect rules:
   ```
   frogsfolly.com {
       # Legacy query string routes
       @legacy_c42 query c=42
       redir @legacy_c42 /about/ 301

       # Legacy .aspx file routes
       redir /about/page.aspx /about/ 301

       # Migrated PageAlias entries
       redir /old-path /new-path 301
   }
   ```
4. Include in auto-generated Caddyfile

### 13.4 Image Migration

Images are stored on the **legacy IIS web server's filesystem**, organized by each site's `GalleryFolder` path (a per-company property in the Company table).

**Migration steps:**

1. **Phase 0:** Inventory images on the legacy server — list files under each `GalleryFolder`, record counts and sizes.
2. **Copy files:** `rsync` or `scp` from legacy server to per-site media folders:
   ```
   # For each site, based on Company.GalleryFolder:
   rsync -av legacy-server:/path/to/{GalleryFolder}/ /var/wpm/data/{domain}/media/
   ```
3. **Verify:** Cross-check `Image.ImageFileName` values against copied files. Flag any missing files.
4. **Do NOT rewrite image paths in HTML during initial migration.** Migrate content verbatim first, get it publishing, then iterate on path cleanup. The existing `GalleryFolder` relative paths may need adjusting to `/media/` — but do this as a post-migration batch step.
5. For each site: images in `{site}/media/` served via Caddy `handle /media/*` block

### 13.5 Content HTML Cleanup (Iterative, Post-Migration)

Legacy `Page.LocationBody` and `Article.ArticleBody` contain raw CKEditor HTML. **Do not attempt to clean this during initial migration.** Migrate verbatim, publish, review visually, then fix iteratively.

Known issues to address post-migration:
- Hardcoded absolute URLs (e.g., `http://frogsfolly.com/gallery/photo.jpg`) → relative paths
- `<font>`, `<center>`, `<marquee>` tags → strip or wrap in semantic HTML
- Inline styles → can coexist with Tailwind initially
- Internal links using `Default.aspx?c=42&a=15` → rewrite to new slug paths
- Broken image references → fix paths to `/media/`

**Approach:** Build a batch HTML processor that runs after migration, with rules applied incrementally. Each rule is a regex or HTML parser transform. Run, review, iterate.

---

## 14. Gap Analysis

Features in the legacy system that need explicit handling:

| # | Feature | Current Implementation | New System Approach | Risk | Effort |
|---|---------|----------------------|--------------------|----- |--------|
| 1 | **URL redirects** | PageAlias table + `Default.aspx?c=X` URLs | Caddy redirect rules; migrate PageAlias entries + generate slug mappings | **High** (SEO) | Medium |
| 2 | **Contact forms** | Form.aspx.vb sends SMTP email | API endpoint `POST /api/contact`; sends via SendGrid or SMTP | **High** | Low |
| 3 | **Images/media** | Files on disk in GalleryFolder (location TBD) | Copy to `/var/wpm/data/{site}/media/`; serve via Caddy | **High** | Medium |
| 4 | **404 pages** | Sophisticated 404.ashx | Branded `404.html` per site, generated by CmsPublisher | Medium | Low |
| 5 | **RSS feeds** | RssToolkit + .ashx handlers + cached XML feeds | Generated as static `rss.xml` by CmsPublisher | Medium | Low |
| 6 | **Sitemap** | Dynamic sitemap.aspx | Generated as static `sitemap.xml` by CmsPublisher | Medium | Low |
| 7 | **www redirect** | ApplicationHttpModule | Caddy redirect blocks | Medium | Low |
| 8 | **Content HTML quality** | CKEditor HTML in DB (years of accumulated markup) | **Defer cleanup.** Migrate verbatim, publish, iterate. | Medium | **High** (if done) |
| 9 | **Mineral Collection app** | LINQ to SQL; original Azure SQL is dead; **SQL scripts exist** | WPM.Domain.Minerals plugin; recreate from SQL scripts → SQLite | **High** | **High** |
| 10 | **Recipe app** | LINQ to SQL; original AWS RDS is dead; **SQL scripts exist** | WPM.Domain.Recipes plugin; recreate from SQL scripts → SQLite | Medium | Medium |
| 11 | **Search** | No search in current system | Pagefind (build-time index on static output); defer | Low | Low |
| 12 | **Backup / DR** | Manual | Nightly SQLite backup to Azure Blob; VM snapshot | Medium | Low |
| 13 | **Monitoring** | None | UptimeRobot (free); API health check endpoint | Low | Low |
| 14 | **Local dev** | IIS + Access + Azure SQL + AWS RDS | `dotnet run` + SQLite; no cloud dependency | Low | Low |
| 15 | **Image table + PageImage junction** | Image metadata + junction to Page; CompanyID filtered | New `Images` + `LocationImages` tables in cms.db | Medium | Low |
| 16 | **SiteCategoryType taxonomy** | Category system in ApplicationDAL.GetSiteLinks() | New `Categories` table in cms.db; may need JOIN migration | Medium | Medium |
| 17 | **PageAlias URL rewriting** | LocationAliasList with wildcard + literal matching | Migrate to Caddy redirect rules per site | **High** (SEO) | Medium |
| 18 | **Mineral geography (City/State/Country)** | 3 location tables with lat/long in Azure SQL | Flatten into minerals.db with denormalized location fields | Low | Low |
| 19 | **Recipe comments + ratings** | User-submitted comments, avg ratings, view counts | Static sites can't accept comments; need API endpoint or drop | Medium | Medium |
| 20 | **Schema differences across .mdb files** | Not all 16 databases have identical schemas | Phase 0 schema audit required; migration tool must handle variations | **High** | Medium |

---

## 15. Risk Register

| # | Risk | Likelihood | Impact | Mitigation |
|---|------|-----------|--------|------------|
| R1 | **Schema differences across .mdb files** — not all 16 databases have identical tables/columns | High | High | Phase 0 schema audit. Migration tool must handle missing tables gracefully. |
| R2 | **CompanyID data partitioning is wrong** — Location/Article filtering may use GroupID or other indirect mechanisms, not just CompanyID | Medium | High | Phase 0: run `SELECT CompanyID, COUNT(*) FROM Page GROUP BY CompanyID` on every .mdb. Verify data volumes match expectations. |
| R3 | **Images are missing or broken paths** — physical image files are on the legacy IIS server filesystem but `Image.ImageFileName` values may not match actual file paths | Medium | High | Phase 0: inventory files on legacy server, cross-check against `Image.ImageFileName` and `Company.GalleryFolder` values in each database. |
| R4 | **Content HTML breaks in new templates** — 20 years of CKEditor output with inline styles, absolute URLs, broken tags | High | Medium | Migrate HTML verbatim. Don't clean during initial migration. Iterate post-publish. |
| R5 | ~~Azure SQL / AWS RDS credentials are stale~~ | ~~Medium~~ | ~~High~~ | **Resolved:** Both databases are confirmed inaccessible. SQL scripts to recreate them exist. Migration will use the SQL scripts, not live connections. |
| R6 | **OLE DB provider unavailable** — `Microsoft.ACE.OLEDB.12.0` must be installed on the build machine | Low | High | Migration tool is Windows-only. Verify provider installed before starting. Document as a prerequisite. |
| R7 | **SEO traffic loss from broken redirects** — missing or incorrect 301 redirect rules | High | High | Generate redirect map during migration. Test every legacy URL pattern. Keep legacy site running for 2+ weeks post-cutover. |
| R8 | **SQLite concurrency during publish** — admin writes + publish reads on same file | Medium | Medium | WAL mode on all SQLite connections. Publish pipeline is read-only. |
| R9 | **Plugin infrastructure delays actual delivery** — weeks spent on interfaces/reflection before a single page publishes | Medium | High | Build CMS as hardcoded integration first. Extract plugin interfaces after 2+ working domains. |
| R10 | **No rollback path** — DNS cutover with no way to revert | Medium | **Critical** | Keep legacy IIS site running at its IP for 2+ weeks. DNS can be reverted within minutes. Document rollback procedure. |

---

## 16. Implementation Phases

### Phase 0 — Discovery & Schema Audit (Week 1)

> **This phase is a prerequisite. Do not write greenfield code until Phase 0 is complete.**
> The migration is the riskiest part of this project. Everything else (Caddy, ASP.NET Core,
> Scriban) is well-understood technology. The hard part is understanding 20 years of Access data.

- [ ] **Dump .mdb schemas**: Run `OleDbConnection.GetSchema()` on all 16 databases. Output CSV: database, table, column, type, nullable. Compare across databases — document which share identical schemas and which diverge.
- [ ] **Map CompanyID → data volume**: For each .mdb, run `SELECT CompanyID, COUNT(*) FROM Page GROUP BY CompanyID` (and Article, Image, Part, Parameter). Verify these match the XML config file mappings.
- [ ] **Verify the `Page` table has CompanyID**: Confirm that filtering by `Page.CompanyID` correctly partitions data for shared databases like FrogsFollyKids.mdb (7 companies in one file).
- [ ] **Identify undocumented tables**: Look for tables not mentioned in ApplicationDAL.vb — there may be tables for contacts, roles, user sessions, etc. Decide which to migrate.
- [ ] **Inventory images**: Query `Image.ImageFileName` and `Company.GalleryFolder` across all databases. Locate the physical files — are they on the current IIS server? In a GalleryFolder path? In Azure Blob Storage?
- [ ] **Validate mineral SQL scripts**: Run the provided SQL scripts locally (SQL Server LocalDB or SQLite). Verify tables match the LINQ to SQL model in `wpmMineralCollection/DataClasses1.designer.vb`. Count rows: CollectionItem, Mineral, CollectionItemImage.
- [ ] **Validate recipe SQL scripts**: Run the provided SQL scripts locally. Verify tables match `WPMRecipe/RecipeLibrary.designer.vb`. Count rows: Recipe, RecipeImage, RecipeComment, RecipeCategory.
- [ ] **Inventory images on legacy web server**: SSH/RDP to legacy IIS server. List all files under each site's `GalleryFolder` path. Record total file count, total size, directory structure per site. Verify `Image.ImageFileName` values match actual files on disk.
- [ ] **Document PageAlias entries**: Query `PageAlias` table across all databases. Count total redirect rules. These are critical for SEO continuity.
- [ ] **Classify the 16 databases**: Categorize each as: (a) active CMS, (b) CMS + minerals, (c) CMS + recipes, (d) backup/template (skip), (e) unknown.

**Deliverable:** A migration readiness report with:
- Complete schema inventory (tables × columns × databases)
- CompanyID → domain mapping (verified against XML configs)
- Data volume per site (row counts)
- Image filesystem inventory (file count, size, paths per site from legacy server)
- SQL script validation results (minerals and recipes table/row counts)
- List of databases to skip (backups, templates)

### Phase 1 — Foundation + CMS Migration (Weeks 2–4)

- [ ] Create solution structure (`WPM.sln`, core projects)
- [ ] Implement `CoreDbContext` (sites, users, auth — shared)
- [ ] Implement `CmsDbContext` with entities mapped from Phase 0 schema findings
- [ ] Implement `SiteMiddleware` (Host header → site folder path)
- [ ] Build `WPM.Migration` tool Phase A (Access → SQLite CMS content)
  - [ ] Company → core.db Sites + SiteDomains + site folders
  - [ ] Page → cms.db Locations (preserving CompanyID partitioning)
  - [ ] Article → cms.db Articles
  - [ ] Part, Parameter, Image, PageImage, PageAlias
  - [ ] Generate site.json from Company row
- [ ] Validate migration: row counts, FK integrity, spot checks
- [ ] Implement JWT auth with user roles
- [ ] Set up Scriban template engine
- [ ] Create CMS CRUD API endpoints (direct, no plugin interfaces yet)
- [ ] Verify: API starts, CRUD works against migrated per-site cms.db

### Phase 2 — Publishing & Static Output (Weeks 3–5)

- [ ] Implement `PublishingService` (direct, not plugin-based yet)
- [ ] Implement `CmsPublisher` (pages, sitemap, RSS, 404)
- [ ] Build Scriban templates (layout, page, article, sitemap)
- [ ] Apply Tailwind CSS to templates
- [ ] Implement contact form API endpoint
- [ ] Generate URL redirect rules from PageAlias + Page data
- [ ] Verify: publish command generates correct static HTML for 2–3 test sites
- [ ] Visual review: compare published output against live legacy sites

### Phase 3 — Admin UI MVP (Weeks 4–6)

- [ ] Build minimal admin (Razor Pages or React — see Section 10.0 decision)
- [ ] Site picker, location tree editor, article editor
- [ ] Publish button (triggers API)
- [ ] Verify: edit content → publish → see changes on site

### Phase 4 — Mineral Collection Domain (Weeks 5–7)

- [ ] Create `WPM.Domain.Minerals` — entities from Phase 0 SQL script schema
- [ ] Build `WPM.Migration` Phase B (SQL scripts → local temp DB → minerals.db)
- [ ] Build specimen templates (collection index, specimen detail, data.json)
- [ ] Copy specimen images from legacy server to site media folder
- [ ] Extract common patterns between CMS and Minerals into `IContentDomain` interface
- [ ] Verify: jmshawminerals.com publishes correctly

### Phase 5 — Recipe Domain (Weeks 6–8, if active)

- [ ] Confirm recipe sites are still active (Open Question #4)
- [ ] Create `WPM.Domain.Recipes` — entities from Phase 0 SQL script schema
- [ ] Build `WPM.Migration` Phase C (SQL scripts → local temp DB → recipes.db)
- [ ] Build recipe templates
- [ ] Decide: recipe comments/ratings — API endpoint or drop?
- [ ] Verify: recipe pages publish correctly

### Phase 6 — Infrastructure & Deployment (Weeks 6–8, parallel)

- [ ] Provision Azure Linux VM
- [ ] Install Caddy + .NET runtime
- [ ] Write systemd service file
- [ ] Generate Caddyfile from core.db (include redirect rules)
- [ ] Set up GitHub Actions CI/CD
- [ ] Write deployment scripts (rsync API, admin, sites, data)
- [ ] Set up nightly SQLite backup (zip per site folder → Azure Blob)
- [ ] Set up UptimeRobot monitoring + API health check endpoint

### Phase 7 — Staging & Cutover (Weeks 8–10)

- [ ] Deploy everything to VM
- [ ] Run full publish for all sites
- [ ] **Staging validation checklist:**
  - [ ] Every domain serves correct content (spot-check 5+ pages per site)
  - [ ] SSL certs obtained for all domains (Caddy auto-SSL)
  - [ ] URL redirects work (test 10+ legacy URLs per site)
  - [ ] Images load correctly
  - [ ] Contact forms submit successfully
  - [ ] Admin UI can edit + republish
  - [ ] 404 pages render per-site branding
  - [ ] sitemap.xml and rss.xml validate
- [ ] **Rollback preparation:**
  - [ ] Document legacy IIS server IP and current DNS records
  - [ ] Verify legacy site still running and accessible by IP
  - [ ] Write DNS rollback script (revert all domains in < 5 minutes)
  - [ ] Set DNS TTL to 300s (5 min) one week before cutover
- [ ] **Cut DNS** (batch by traffic level — lowest traffic first)
  - [ ] Batch 1: 5 lowest-traffic sites (monitor 24h)
  - [ ] Batch 2: 10 medium-traffic sites (monitor 24h)
  - [ ] Batch 3: remaining sites including highest-traffic
- [ ] Monitor for issues (check UptimeRobot, review Caddy logs)
- [ ] **Keep legacy IIS running for 2+ weeks** as rollback target
- [ ] Decommission legacy VM only after 2 weeks with zero rollback needs

---

## 17. Open Questions

> Track decisions made and questions pending.

| # | Question | Status | Decision |
|---|----------|--------|----------|
| 1 | Which Scriban vs RazorLight for templates? | **Open** | Leaning Scriban (lighter, no ASP.NET dependency) |
| 2 | Image storage: where? | **Decided** | Per-site: `/var/wpm/data/{domain}/media/` |
| 3 | How many admin users? Just one or multiple editors? | **Open** | Affects auth complexity; `User.AuthorizedSiteIds` allows per-site access |
| 4 | Is the Recipe project still active? | **Open** | Determines if we build WPM.Domain.Recipes; Phase 0 should check if recipe sites get traffic |
| 5 | Mineral Collection: is the pivot table used? | **Open** | Affects whether we need client-side JS for filtering |
| 6 | Do any public sites need login/registration? | **Open** | If no, auth is admin-only (simpler) |
| 7 | Should Caddyfile be auto-generated or hand-maintained? | **Open** | Leaning auto-generated from `core.db` |
| 8 | B1s (1 GB RAM) vs B1ms (2 GB RAM)? | **Open** | Start B1s, upgrade if needed |
| 9 | Tailwind build: during CI or pre-built? | **Open** | Leaning CI (GitHub Actions) |
| 10 | Do we need Yelp integration or Twitter/RSS aggregation? | **Open** | Legacy features; likely dead code. localhost/xml/ has cached RSS feeds (Bing Travel, Flickr) |
| 11 | Rich text editor for admin: TipTap vs Lexical? | **Open** | Defer until admin UI phase |
| 12 | Search: add Pagefind now or defer? | **Open** | Leaning defer (not in current system) |
| 13 | Data isolation model? | **Decided** | Per-site folders with per-domain `.db` files; no TenantId columns |
| 14 | How do sites share templates/themes? | **Open** | Options: (a) shared template dir, (b) per-site template overrides, (c) theme packages |
| 15 | Should `site.json` be in the DB or on disk? | **Decided** | On disk in site folder — easy to edit, version, and backup |
| 16 | EF Core migrations for per-domain DBs? | **Open** | Options: (a) code-first migrations per domain, (b) `dotnet wpm migrate` CLI tool, (c) manual SQL scripts |
| 17 | How to handle media URLs in published HTML? | **Open** | Options: (a) relative `/media/`, (b) CDN URL if added later, (c) configurable base URL in `site.json` |
| 18 | Migration source: API or direct DB? | **Decided** | **Direct database access.** Legacy APIs are thin runtime endpoints, not export APIs. CMS data from .mdb files via OLE DB. Minerals and recipes from provided SQL scripts. |
| 19 | Are Azure SQL (minerals) and AWS RDS (recipes) still accessible? | **Resolved** | **No — both are dead.** SQL scripts to recreate the databases exist. Migration will run the scripts into temp local DB (SQL Server LocalDB or adapt to SQLite), then migrate from there. |
| 20 | Legacy DB table is `Page`, not `Location` — does this affect CMS entity naming? | **Decided** | New system uses `Location` as entity name (matches the VB.NET in-memory class). Migration maps `Page` → `Location`. |
| 21 | Admin UI approach: React SPA or Razor Pages MVP? | **Open** | Razor Pages is faster to build for MVP. React SPA is better long-term. Decision affects Phase 3 timeline by 2–4 weeks. |
| 22 | Which .mdb files are backups vs active? | **Phase 0** | `FrogsFollyBase.mdb`, `1-MineralCollection.mdb`, `wpm-demo.mdb`, `FrogsFolly.mdb` (vs `wpmFrogsFolly.mdb`) need classification. |
| 23 | `LINQHelper` project — is it used in data access? | **Phase 0** | Check if any production code depends on it. If not, exclude from migration scope. |
| 24 | Recipe comments on static sites — keep or drop? | **Open** | Static sites can't accept user-submitted comments. Options: (a) drop comments, (b) add API endpoint + JS widget, (c) use Disqus/similar |
| 25 | Where are physical image files stored? | **Resolved** | **On the legacy IIS web server filesystem**, organized by `Company.GalleryFolder` paths. Phase 0 will inventory actual files and cross-check against `Image.ImageFileName` values in the databases. |

---

## Revision History

| Version | Date | Changes |
|---------|------|---------|
| 0.1 | Feb 2026 | Initial draft from architecture discussion |
| 0.2 | Feb 2026 | Per-site folder structure replaces shared DB with TenantId; each domain owns its own DbContext and SQLite file per site; added ISchemaContract for CMS integration validation; SiteContext replaces PublishContext; SiteMiddleware replaces TenantMiddleware; added site.json config; updated all code examples throughout |
| 0.3 | Feb 2026 | **Critical review.** Major findings from codebase analysis: (1) Legacy DB table is `Page`, not `Location` — the `Location` class is in-memory only; (2) Mineral data lives on Azure SQL, not in .mdb files; (3) Recipe data lives on AWS RDS, not in .mdb files; (4) Added Phase 0 — Discovery & Schema Audit as prerequisite; (5) Rewrote Section 13 migration with 3-source strategy (.mdb + Azure SQL + AWS RDS); (6) Added Risk Register (Section 15); (7) Added rollback plan to cutover phase; (8) Added plugin pragmatism note — build CMS first, extract interfaces later; (9) Added admin UI phasing — Razor Pages MVP option; (10) Added SQLite WAL mode requirement; (11) Updated Gap Analysis with 6 new items; (12) Updated Open Questions with 7 new items including blocking items for Phase 0; (13) Fixed legacy table mapping throughout (Page, Image, PageImage, PageAlias, SiteCategoryType); (14) Added CompanyID data partitioning documentation for shared .mdb files |
| 0.4 | Feb 2026 | **Blocking questions resolved.** (1) Azure SQL and AWS RDS confirmed dead — SQL scripts exist to recreate both databases; migration uses scripts, not live connections; (2) Images confirmed on legacy IIS web server filesystem, organized by `Company.GalleryFolder`; migration uses rsync/scp from legacy server; (3) Updated migration Phases B/C from "connect to remote DB" to "run SQL scripts locally"; (4) Updated risk R5 as resolved; (5) Resolved open questions #19 and #25; (6) No more blocking items — Phase 0 can proceed |
