using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace WPM.Domain.CMS.Data;

public static class CmsDbContextFactory
{
    public const string DatabaseFileName = "cms.db";

    public static CmsDbContext Create(string siteDataFolder)
    {
        var dbPath = Path.Combine(siteDataFolder, DatabaseFileName);
        var context = new CmsDbContext(dbPath);
        context.Database.EnsureCreated();

        var connection = context.Database.GetDbConnection() as SqliteConnection;
        if (connection is not null)
        {
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "PRAGMA journal_mode=WAL; PRAGMA busy_timeout=5000;";
            cmd.ExecuteNonQuery();
        }

        return context;
    }
}
