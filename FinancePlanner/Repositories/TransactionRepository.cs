using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Transactions;

public class TransactionRepository
{
    private const string connectionString = "Data Source=finance.db";

    public void Add(Transaction transaction)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
        INSERT INTO Transactions (Amount, Date, CategoryId, Type, Description)
        VALUES ($amount, $date, $categoryId, $type, $description);
        ";

        command.Parameters.AddWithValue("$amount", transaction.Amount);
        command.Parameters.AddWithValue("$date", transaction.Date.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("$categoryId", transaction.CategoryId);
        command.Parameters.AddWithValue("$type", transaction.Type);
        command.Parameters.AddWithValue("$description", transaction.Description);

        command.ExecuteNonQuery();
    }

    public List<Transaction> GetAll()
    {
        var list = new List<Transaction>();

        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Transactions";

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new Transaction
            {
                Id = reader.GetInt32(0),
                Amount = reader.GetDecimal(1),
                Date = DateTime.Parse(reader.GetString(2)),
                CategoryId = reader.GetInt32(3),
                Type = reader.GetString(4),
                Description = reader.IsDBNull(5) ? "" : reader.GetString(5)
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