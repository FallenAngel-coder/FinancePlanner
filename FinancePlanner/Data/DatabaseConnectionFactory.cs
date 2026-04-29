using Microsoft.Data.Sqlite;

public static class DatabaseConnectionFactory
{
    private const string ConnectionString = "Data Source=finance.db";

    public static SqliteConnection CreateConnection()
    {
        var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        return connection;
    }
}