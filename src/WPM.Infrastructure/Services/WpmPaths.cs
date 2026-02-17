namespace WPM.Infrastructure.Services;

public class WpmPaths
{
    public required string DataRoot { get; init; }
    public required string SitesRoot { get; init; }
    public required string CoreDbPath { get; init; }

    public static WpmPaths FromBaseDirectory(string baseDir) => new()
    {
        DataRoot = Path.Combine(baseDir, "data"),
        SitesRoot = Path.Combine(baseDir, "sites"),
        CoreDbPath = Path.Combine(baseDir, "core.db")
    };
}
