using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Xml.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WPM.Infrastructure.Data;
using WPM.Infrastructure.Services;
using WPM.Migration;

// WPM Migration Tool — Multi-phase migration from legacy Access to greenfield SQLite

var command = args.Length > 0 ? args[0].ToLowerInvariant() : "help";
var repoRoot = args.Length > 1 ? args[1] : FindRepoRoot();

switch (command)
{
    case "phase0":
        await RunPhase0(repoRoot);
        break;
    case "migrate":
        await RunMigrate(repoRoot);
        break;
    case "validate":
        await RunValidate(repoRoot);
        break;
    default:
        Console.WriteLine("WPM Migration Tool");
        Console.WriteLine("==================");
        Console.WriteLine();
        Console.WriteLine("Usage: dotnet run -- <command> [repoRoot]");
        Console.WriteLine();
        Console.WriteLine("Commands:");
        Console.WriteLine("  phase0    Discovery & schema audit (read-only)");
        Console.WriteLine("  migrate   Access → SQLite CMS content migration");
        Console.WriteLine("  validate  Validate migration output (row counts, FK integrity)");
        Console.WriteLine();
        Console.WriteLine("If repoRoot is not specified, auto-detects from .git directory.");
        break;
}

// ═══════════════════════════════════════════════════════════════════════
// Phase A: Migrate Access → SQLite
// ═══════════════════════════════════════════════════════════════════════

async Task RunMigrate(string root)
{
    Console.WriteLine("WPM Migration Tool — Phase A: Access → SQLite Migration");
    Console.WriteLine("========================================================");

    var appDataPath = Path.Combine(root, "archive", "website", "App_Data");
    var sitesConfigPath = Path.Combine(appDataPath, "Sites");
    var outputBase = Path.Combine(root, "migration-output");

    Console.WriteLine($"Source:  {appDataPath}");
    Console.WriteLine($"Output:  {outputBase}");

    // Auto-clear previous migration output for idempotent re-runs
    if (Directory.Exists(outputBase))
    {
        Console.WriteLine("Clearing previous migration output...");
        Directory.Delete(outputBase, true);
    }
    Console.WriteLine();

    // Parse XML configs
    var configs = ParseXmlConfigs(sitesConfigPath);
    Console.WriteLine($"Found {configs.Count} site configs");

    // Filter out localhost (dev-only) and deduplicate by (MDB, CompanyId)
    configs = configs
        .Where(c => !c.Domain.Equals("localhost", StringComparison.OrdinalIgnoreCase))
        .ToList();

    // Deduplicate: if multiple XML configs point to same MDB+CompanyID, keep first non-localhost
    configs = configs
        .GroupBy(c => (MdbFile: Path.GetFileName(c.DbPath).ToLowerInvariant(), c.CompanyId))
        .Select(g => g.First())
        .ToList();

    Console.WriteLine($"After dedup/filter: {configs.Count} sites to migrate");
    Console.WriteLine();

    // Set up output paths
    var paths = WpmPaths.FromBaseDirectory(outputBase);
    Directory.CreateDirectory(paths.DataRoot);
    Directory.CreateDirectory(paths.SitesRoot);

    // Create and initialize core.db
    var coreDbOptions = new DbContextOptionsBuilder<CoreDbContext>()
        .UseSqlite($"Data Source={paths.CoreDbPath}")
        .Options;
    using var coreDb = new CoreDbContext(coreDbOptions);
    await coreDb.Database.EnsureCreatedAsync();

    // Enable WAL on core.db
    var coreConn = coreDb.Database.GetDbConnection() as SqliteConnection;
    if (coreConn is not null)
    {
        if (coreConn.State != ConnectionState.Open) await coreConn.OpenAsync();
        using var cmd = coreConn.CreateCommand();
        cmd.CommandText = "PRAGMA journal_mode=WAL; PRAGMA busy_timeout=5000;";
        await cmd.ExecuteNonQueryAsync();
    }

    // Group configs by MDB file to minimize file opens
    var mdbGroups = configs.GroupBy(c => Path.GetFileName(c.DbPath).ToLowerInvariant());
    var results = new List<MigrationResult>();

    foreach (var group in mdbGroups)
    {
        var mdbFileName = group.Key;
        var mdbFullPath = Path.Combine(appDataPath, mdbFileName);

        if (!File.Exists(mdbFullPath))
        {
            Console.WriteLine($"WARNING: MDB file not found: {mdbFullPath}");
            continue;
        }

        Console.WriteLine($"Opening {mdbFileName}...");
        using var reader = new MdbReader(mdbFullPath);

        foreach (var config in group.OrderBy(c => c.Domain))
        {
            try
            {
                var legacyConfig = new LegacySiteConfig(
                    config.XmlFileName, config.Domain, config.DbPath, config.CompanyId);
                var result = await SiteMigrator.MigrateAsync(reader, legacyConfig, coreDb, paths);
                results.Add(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ERROR migrating {config.Domain}: {ex.Message}");
                var errorResult = new MigrationResult(config.Domain);
                errorResult.AddWarning($"Migration failed: {ex.Message}");
                results.Add(errorResult);
            }
        }
    }

    // Print summary
    Console.WriteLine();
    Console.WriteLine("═══ Migration Summary ═══");
    Console.WriteLine($"Sites processed: {results.Count}");
    Console.WriteLine($"Total locations: {results.Sum(r => r.MigratedLocationCount)}");
    Console.WriteLine($"Total articles:  {results.Sum(r => r.MigratedArticleCount)}");
    Console.WriteLine($"Total images:    {results.Sum(r => r.MigratedImageCount)}");
    Console.WriteLine($"Total loc-imgs:  {results.Sum(r => r.MigratedLocationImageCount)}");
    Console.WriteLine($"Total aliases:   {results.Sum(r => r.MigratedAliasCount)}");

    var warnings = results.SelectMany(r => r.Warnings).ToList();
    if (warnings.Count > 0)
    {
        Console.WriteLine($"\nWarnings ({warnings.Count}):");
        foreach (var w in warnings)
            Console.WriteLine($"  - {w}");
    }

    Console.WriteLine();
    Console.WriteLine($"Output written to: {outputBase}");
    Console.WriteLine("Run 'dotnet run -- validate' to verify migration integrity.");
}

// ═══════════════════════════════════════════════════════════════════════
// Validate
// ═══════════════════════════════════════════════════════════════════════

async Task RunValidate(string root)
{
    var outputBase = Path.Combine(root, "migration-output");
    var paths = WpmPaths.FromBaseDirectory(outputBase);
    var reportPath = Path.Combine(root, ".documentation", "MIGRATION_VALIDATION_REPORT.md");

    if (!File.Exists(paths.CoreDbPath))
    {
        Console.WriteLine($"ERROR: core.db not found at {paths.CoreDbPath}");
        Console.WriteLine("Run 'dotnet run -- migrate' first.");
        return;
    }

    var coreDbOptions = new DbContextOptionsBuilder<CoreDbContext>()
        .UseSqlite($"Data Source={paths.CoreDbPath}")
        .Options;
    using var coreDb = new CoreDbContext(coreDbOptions);

    await MigrationValidator.ValidateAsync(coreDb, paths, reportPath);
}

// ═══════════════════════════════════════════════════════════════════════
// Phase 0: Discovery (preserved from original)
// ═══════════════════════════════════════════════════════════════════════

async Task RunPhase0(string root)
{
    var appDataPath = Path.Combine(root, "archive", "website", "App_Data");
    var sitesConfigPath = Path.Combine(appDataPath, "Sites");
    var reportPath = Path.Combine(root, ".documentation", "PHASE0_REPORT.md");

    Console.WriteLine("WPM Migration Tool — Phase 0: Discovery & Schema Audit");
    Console.WriteLine("=======================================================");
    Console.WriteLine($"App_Data: {appDataPath}");
    Console.WriteLine();

    var report = new StringBuilder();
    report.AppendLine("# Phase 0 — Migration Readiness Report");
    report.AppendLine();
    report.AppendLine($"**Generated:** {DateTime.Now:yyyy-MM-dd HH:mm}");
    report.AppendLine($"**Source:** `archive/website/App_Data/`");
    report.AppendLine();

    // Step 1: Parse XML configs
    Console.WriteLine("Step 1: Parsing XML site configs...");
    var configs = ParseXmlConfigs(sitesConfigPath);

    report.AppendLine("---");
    report.AppendLine();
    report.AppendLine("## 1. Site Configuration Map (from XML configs)");
    report.AppendLine();
    report.AppendLine($"**Total domains configured:** {configs.Count}");
    report.AppendLine();
    report.AppendLine("| Domain | CompanyID | MDB File | Config File |");
    report.AppendLine("|--------|----------|----------|-------------|");
    foreach (var c in configs.OrderBy(c => c.DbPath).ThenBy(c => c.CompanyId))
    {
        var mdbName = Path.GetFileName(c.DbPath);
        report.AppendLine($"| {c.Domain} | {c.CompanyId} | {mdbName} | {c.XmlFileName}.xml |");
    }
    report.AppendLine();

    var dbGroups = configs.GroupBy(c => Path.GetFileName(c.DbPath)).OrderBy(g => g.Key);
    report.AppendLine("### Databases by Domain Count");
    report.AppendLine();
    report.AppendLine("| MDB File | Domains | CompanyIDs |");
    report.AppendLine("|----------|---------|------------|");
    foreach (var g in dbGroups)
    {
        var ids = string.Join(", ", g.OrderBy(c => c.CompanyId).Select(c => c.CompanyId));
        report.AppendLine($"| {g.Key} | {g.Count()} | {ids} |");
    }
    report.AppendLine();
    Console.WriteLine($"  Found {configs.Count} domain configs across {dbGroups.Count()} databases");

    // Step 2: Referenced vs unreferenced MDBs
    Console.WriteLine("Step 2: Identifying referenced vs unreferenced .mdb files...");
    var allMdbFiles = Directory.GetFiles(appDataPath, "*.mdb").Select(Path.GetFileName).ToHashSet(StringComparer.OrdinalIgnoreCase);
    var referencedMdbFiles = configs.Select(c => Path.GetFileName(c.DbPath)).ToHashSet(StringComparer.OrdinalIgnoreCase);
    var unreferencedMdbFiles = allMdbFiles.Except(referencedMdbFiles, StringComparer.OrdinalIgnoreCase).OrderBy(f => f).ToList();

    report.AppendLine("### Database Classification");
    report.AppendLine();
    report.AppendLine("| MDB File | Size (KB) | Status |");
    report.AppendLine("|----------|----------|--------|");
    foreach (var mdb in allMdbFiles.OrderBy(f => f))
    {
        var fullPath = Path.Combine(appDataPath, mdb!);
        var sizeKb = new FileInfo(fullPath).Length / 1024;
        var status = referencedMdbFiles.Contains(mdb!) ? "Active (referenced by XML config)" : "**Unreferenced** — backup/template?";
        report.AppendLine($"| {mdb} | {sizeKb:N0} | {status} |");
    }
    report.AppendLine();
    Console.WriteLine($"  {referencedMdbFiles.Count} referenced, {unreferencedMdbFiles.Count} unreferenced");

    // Step 3: Schema dump
    Console.WriteLine("Step 3: Dumping schemas from all .mdb files...");
    report.AppendLine("---");
    report.AppendLine();
    report.AppendLine("## 2. Schema Inventory");
    report.AppendLine();

    foreach (var mdbFile in allMdbFiles.OrderBy(f => f))
    {
        var fullPath = Path.Combine(appDataPath, mdbFile!);
        var connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={fullPath};";
        report.AppendLine($"### {mdbFile}");
        report.AppendLine();

        try
        {
            using var conn = new OleDbConnection(connStr);
            conn.Open();
            var tables = conn.GetSchema("Tables").AsEnumerable()
                .Where(r => r["TABLE_TYPE"].ToString() == "TABLE")
                .Select(r => r["TABLE_NAME"].ToString()!)
                .Where(t => !t.StartsWith("MSys"))
                .OrderBy(t => t).ToList();

            report.AppendLine("| Table | Columns | Row Count |");
            report.AppendLine("|-------|---------|-----------|");

            foreach (var table in tables)
            {
                var columns = conn.GetSchema("Columns", [null, null, table]).AsEnumerable()
                    .Select(r => new { Name = r["COLUMN_NAME"].ToString()!, Type = r["DATA_TYPE"]?.ToString() ?? "" })
                    .OrderBy(c => c.Name).ToList();
                int rowCount = 0;
                try { using var cmd = new OleDbCommand($"SELECT COUNT(*) FROM [{table}]", conn); rowCount = Convert.ToInt32(cmd.ExecuteScalar()); } catch { }
                report.AppendLine($"| {table} | {columns.Count} | {rowCount:N0} |");
            }
            report.AppendLine();
        }
        catch (Exception ex)
        {
            report.AppendLine($"**ERROR:** Could not open database: {ex.Message}");
            report.AppendLine();
        }
    }

    // Step 4: CompanyID data volume
    Console.WriteLine("Step 4: Mapping CompanyID → data volume...");
    report.AppendLine("---");
    report.AppendLine();
    report.AppendLine("## 3. CompanyID → Data Volume");
    report.AppendLine();

    var companyData = new List<(string MdbFile, string Table, int CompanyId, int Count)>();
    foreach (var mdbFile in referencedMdbFiles.OrderBy(f => f))
    {
        var fullPath = Path.Combine(appDataPath, mdbFile);
        var connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={fullPath};";
        try
        {
            using var conn = new OleDbConnection(connStr);
            conn.Open();
            foreach (var table in new[] { "Page", "Article", "Image", "PageAlias", "Part", "Parameter" })
            {
                try
                {
                    using var cmd = new OleDbCommand($"SELECT CompanyID, COUNT(*) as Cnt FROM [{table}] GROUP BY CompanyID", conn);
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                        companyData.Add((mdbFile, table, Convert.ToInt32(reader["CompanyID"]), Convert.ToInt32(reader["Cnt"])));
                }
                catch { }
            }
        }
        catch (Exception ex) { Console.WriteLine($"  WARNING: Failed to query {mdbFile}: {ex.Message}"); }
    }

    report.AppendLine("| MDB File | CompanyID | Domain | Pages | Articles | Images | Aliases |");
    report.AppendLine("|----------|----------|--------|-------|----------|--------|---------|");
    foreach (var config in configs.OrderBy(c => c.DbPath).ThenBy(c => c.CompanyId))
    {
        var mdb = Path.GetFileName(config.DbPath);
        var rows = companyData.Where(d => d.MdbFile.Equals(mdb, StringComparison.OrdinalIgnoreCase) && d.CompanyId == config.CompanyId);
        report.AppendLine($"| {mdb} | {config.CompanyId} | {config.Domain} | " +
            $"{rows.FirstOrDefault(r => r.Table == "Page").Count} | {rows.FirstOrDefault(r => r.Table == "Article").Count} | " +
            $"{rows.FirstOrDefault(r => r.Table == "Image").Count} | {rows.FirstOrDefault(r => r.Table == "PageAlias").Count} |");
    }
    report.AppendLine();

    // Summary
    report.AppendLine("## 4. Summary");
    report.AppendLine();
    report.AppendLine($"| Total domains | {configs.Count} |");
    report.AppendLine($"| Total Pages | {companyData.Where(d => d.Table == "Page").Sum(d => d.Count):N0} |");
    report.AppendLine($"| Total Articles | {companyData.Where(d => d.Table == "Article").Sum(d => d.Count):N0} |");
    report.AppendLine($"| Total Images | {companyData.Where(d => d.Table == "Image").Sum(d => d.Count):N0} |");

    await File.WriteAllTextAsync(reportPath, report.ToString());
    Console.WriteLine($"\nReport written to: {reportPath}");
    Console.WriteLine("Phase 0 discovery complete.");
}

// ═══════════════════════════════════════════════════════════════════════
// Shared helpers
// ═══════════════════════════════════════════════════════════════════════

List<SiteConfig> ParseXmlConfigs(string sitesConfigPath)
{
    var configs = new List<SiteConfig>();
    foreach (var xmlFile in Directory.GetFiles(sitesConfigPath, "*.xml"))
    {
        try
        {
            var doc = XDocument.Load(xmlFile);
            var config = doc.Root?.Element("Configuration");
            if (config is null) continue;

            configs.Add(new SiteConfig(
                Path.GetFileNameWithoutExtension(xmlFile),
                config.Element("DomainName")?.Value ?? "",
                config.Element("AccessDatabasePath")?.Value ?? "",
                int.TryParse(config.Element("CompanyID")?.Value, out var cid) ? cid : 0
            ));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  WARNING: Failed to parse {Path.GetFileName(xmlFile)}: {ex.Message}");
        }
    }
    return configs;
}

static string FindRepoRoot()
{
    var dir = Directory.GetCurrentDirectory();
    while (dir is not null)
    {
        if (Directory.Exists(Path.Combine(dir, ".git")))
            return dir;
        dir = Directory.GetParent(dir)?.FullName;
    }
    return Directory.GetCurrentDirectory();
}

// Record types used by Phase 0 (keep compatible with original)
record SiteConfig(string XmlFileName, string Domain, string DbPath, int CompanyId);
