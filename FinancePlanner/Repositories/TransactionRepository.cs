using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Transactions;
/// <summary>
/// Репозиторій для управління транзакціями в БД SQLite.
/// Реалізує патерн Repository для ізоляції логіки доступу до даних.
/// </summary>
public class TransactionRepository
{
    // Рядок підключення. У майбутньому варто винести в конфігураційний файл.
    private const string connectionString = "Data Source=finance.db";

    public void Add(Transaction transaction)
    {
        // Використовуємо 'using var' для автоматичного закриття з'єднання (Disposable pattern)
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var command = connection.CreateCommand();

        // Використовуємо параметризований запит для захисту від SQL-ін'єкцій
        command.CommandText = @"
            INSERT INTO Transactions (Amount, Date, CategoryId, Type, Description)
            VALUES ($amount, $date, $categoryId, $type, $description);
            ";

        // Додаємо параметри. Дата конвертується у формат ISO для коректного сортування в SQLite
        command.Parameters.AddWithValue("$amount", transaction.Amount);
        command.Parameters.AddWithValue("$date", transaction.Date.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("$categoryId", transaction.CategoryId);
        command.Parameters.AddWithValue("$type", transaction.Type);
        command.Parameters.AddWithValue("$description", transaction.Description ?? (object)DBNull.Value);

        command.ExecuteNonQuery();
    }

    public List<Transaction> GetAll()
    {
        var list = new List<Transaction>();

        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Transactions ORDER BY Date DESC"; // Додав сортування за датою

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            // Мапінг даних з БД на об'єкт моделі Transaction
            list.Add(new Transaction
            {
                Id = reader.GetInt32(0),
                Amount = reader.GetDecimal(1),
                // SQLite повертає дату як рядок, тому парсимо її назад у DateTime
                Date = DateTime.Parse(reader.GetString(2)),
                CategoryId = reader.GetInt32(3),
                Type = reader.GetString(4),
                // Перевірка на null для опису, щоб уникнути NullReferenceException
                Description = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
            });
        }

        return list;
    }

    public void Delete(int id)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Transactions WHERE Id = $id";
        command.Parameters.AddWithValue("$id", id);

        command.ExecuteNonQuery();
    }
}