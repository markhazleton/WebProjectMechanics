// WPM Migration Tool
// Reads from legacy data sources and writes to per-site SQLite databases.
//
// Phase A: Access .mdb → SQLite CMS content (requires Windows + ACE OLE DB driver)
// Phase B: SQL scripts → minerals.db
// Phase C: SQL scripts → recipes.db
// Phase D: Legacy server images → site media folders

Console.WriteLine("WPM Migration Tool");
Console.WriteLine("==================");
Console.WriteLine();
Console.WriteLine("This tool migrates data from legacy sources to the new per-site SQLite structure.");
Console.WriteLine("Run with --help for usage information.");
Console.WriteLine();
Console.WriteLine("Phases:");
Console.WriteLine("  phase-a    Migrate CMS content from .mdb files");
Console.WriteLine("  phase-b    Migrate mineral collection from SQL scripts");
Console.WriteLine("  phase-c    Migrate recipes from SQL scripts");
Console.WriteLine("  phase-d    Copy images from legacy server");
Console.WriteLine();
Console.WriteLine("Not yet implemented. Complete Phase 0 (Discovery & Schema Audit) first.");
