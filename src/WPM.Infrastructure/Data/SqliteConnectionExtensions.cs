using Microsoft.Data.Sqlite;

namespace WPM.Infrastructure.Data;

public static class SqliteConnectionExtensions
{
    public static void EnableWalMode(this SqliteConnection connection)
    {
        if (connection.State != System.Data.ConnectionState.Open)
            connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "PRAGMA journal_mode=WAL; PRAGMA busy_timeout=5000;";
        cmd.ExecuteNonQuery();
    }
}
