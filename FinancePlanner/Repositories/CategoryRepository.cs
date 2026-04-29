using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using FinancePlanner.Services;

namespace FinancePlanner.Repositories
{
    public class CategoryRepository
    {
        public void Add(Category category)
        {
            var connection = DatabaseConnection.Instance.GetConnection();
            var command = connection.CreateCommand();
            
            command.CommandText = "INSERT INTO Categories (Name, Type, UserId) VALUES ($name, $type, $userId)";
            command.Parameters.AddWithValue("$name", category.Name);
            command.Parameters.AddWithValue("$type", category.Type);
            
            int currentUserId = SessionManager.Instance.CurrentUser?.Id ?? 0;
            command.Parameters.AddWithValue("$userId", currentUserId);
            
            command.ExecuteNonQuery();
        }

        public List<Category> GetByType(string type)
        {
            var list = new List<Category>();
            var connection = DatabaseConnection.Instance.GetConnection();
            var command = connection.CreateCommand();
            
            int currentUserId = SessionManager.Instance.CurrentUser?.Id ?? 0;
            command.CommandText = "SELECT Id, Name, Type, UserId FROM Categories WHERE Type = $type AND UserId = $userId ORDER BY Name";
            command.Parameters.AddWithValue("$type", type);
            command.Parameters.AddWithValue("$userId", currentUserId);
            
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Category
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Type = reader.GetString(2),
                    UserId = reader.IsDBNull(3) ? 0 : reader.GetInt32(3)
                });
            }
            
            return list;
        }
    }
}
