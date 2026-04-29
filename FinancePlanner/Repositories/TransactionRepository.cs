using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Transactions;
using FinancePlanner.Services;
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
        // Використовуємо єдине підключення з Singleton
        var connection = DatabaseConnection.Instance.GetConnection();

        var command = connection.CreateCommand();

        // Використовуємо параметризований запит для захисту від SQL-ін'єкцій
        command.CommandText = @"
            INSERT INTO Transactions (Amount, Date, CategoryId, Type, Description, UserId)
            VALUES ($amount, $date, $categoryId, $type, $description, $userId);
            ";

        // Додаємо параметри. Дата конвертується у формат ISO для коректного сортування в SQLite
        command.Parameters.AddWithValue("$amount", transaction.Amount);
        command.Parameters.AddWithValue("$date", transaction.Date.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("$categoryId", transaction.CategoryId == 0 ? (object)DBNull.Value : transaction.CategoryId);
        command.Parameters.AddWithValue("$type", transaction.Type);
        command.Parameters.AddWithValue("$description", transaction.Description ?? (object)DBNull.Value);
        
        int currentUserId = SessionManager.Instance.CurrentUser?.Id ?? 0;
        command.Parameters.AddWithValue("$userId", currentUserId);

        command.ExecuteNonQuery();
    }

    public List<Transaction> GetAll()
    {
        var list = new List<Transaction>();

        var connection = DatabaseConnection.Instance.GetConnection();

        var command = connection.CreateCommand();
        int currentUserId = SessionManager.Instance.CurrentUser?.Id ?? 0;
        command.CommandText = "SELECT * FROM Transactions WHERE UserId = $userId ORDER BY Date DESC"; 
        command.Parameters.AddWithValue("$userId", currentUserId);

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
                // Перевірка на null для CategoryId (ordinal 3)
                CategoryId = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                Type = reader.GetString(4),
                // Перевірка на null для опису, щоб уникнути NullReferenceException
                Description = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                UserId = reader.IsDBNull(6) ? 0 : reader.GetInt32(6)
            });
        }

        return list;
    }

    public void Delete(int id)
    {
        var connection = DatabaseConnection.Instance.GetConnection();

        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Transactions WHERE Id = $id";
        command.Parameters.AddWithValue("$id", id);

        command.ExecuteNonQuery();
    }
}