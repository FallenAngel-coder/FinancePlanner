using System;
using System.Security.Cryptography;
using System.Text;
using FinancePlanner.Models;

namespace FinancePlanner.Repositories
{
    public class UserRepository
    {
        public User Register(string username, string password)
        {
            var connection = DatabaseConnection.Instance.GetConnection();
            var command = connection.CreateCommand();
            
            string hash = HashPassword(password);

            command.CommandText = "INSERT INTO Users (Username, PasswordHash) VALUES ($username, $hash);";
            command.Parameters.AddWithValue("$username", username);
            command.Parameters.AddWithValue("$hash", hash);

            try
            {
                command.ExecuteNonQuery();
                return Login(username, password);
            }
            catch
            {
                // Помилка найімовірніше через дублювання унікального Username
                return null;
            }
        }

        public User Login(string username, string password)
        {
            var connection = DatabaseConnection.Instance.GetConnection();
            var command = connection.CreateCommand();

            string hash = HashPassword(password);

            command.CommandText = "SELECT Id, Username, PasswordHash FROM Users WHERE Username = $username AND PasswordHash = $hash;";
            command.Parameters.AddWithValue("$username", username);
            command.Parameters.AddWithValue("$hash", hash);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    PasswordHash = reader.GetString(2)
                };
            }
            return null;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
