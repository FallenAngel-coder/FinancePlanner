using Microsoft.Data.Sqlite;

public class DatabaseInitializer
{
    private const string connectionString = "Data Source=finance.db";

    public static void Initialize()
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = @"
        CREATE TABLE IF NOT EXISTS Categories (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            Type TEXT NOT NULL
        );

        CREATE TABLE IF NOT EXISTS Transactions (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Amount REAL NOT NULL,
            Date TEXT NOT NULL,
            CategoryId INTEGER,
            Type TEXT NOT NULL,
            Description TEXT,
            FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
        );

        CREATE TABLE IF NOT EXISTS Budgets (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            CategoryId INTEGER,
            [Limit] REAL,
            Month INTEGER,
            Year INTEGER,
            FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
        );
        ";

        command.ExecuteNonQuery();
    }
}