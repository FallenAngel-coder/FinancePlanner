using Microsoft.Data.Sqlite;

public class DatabaseInitializer
{
    private const string connectionString = "Data Source=finance.db";

    public static void Initialize()
    {
        var connection = DatabaseConnection.Instance.GetConnection();


        var command = connection.CreateCommand();

        command.CommandText = @"
        CREATE TABLE IF NOT EXISTS Users (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Username TEXT NOT NULL UNIQUE,
            PasswordHash TEXT NOT NULL
        );

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

        try
        {
            var alterCommand = connection.CreateCommand();
            alterCommand.CommandText = "ALTER TABLE Transactions ADD COLUMN UserId INTEGER DEFAULT 0;";
            alterCommand.ExecuteNonQuery();
        }
        catch { /* Ігноруємо помилку, якщо колонка вже існує */ }

        try
        {
            var alterCategoriesCommand = connection.CreateCommand();
            alterCategoriesCommand.CommandText = "ALTER TABLE Categories ADD COLUMN UserId INTEGER DEFAULT 0;";
            alterCategoriesCommand.ExecuteNonQuery();
        }
        catch { /* Ігноруємо помилку, якщо колонка вже існує */ }
    }
}