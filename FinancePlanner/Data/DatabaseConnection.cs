using Microsoft.Data.Sqlite;

public class DatabaseConnection
{
    private static DatabaseConnection _instance;
    private static readonly object _lock = new object();
    private SqliteConnection _connection;
    private const string connectionString = "Data Source=finance.db";

    private DatabaseConnection()
    {
        _connection = new SqliteConnection(connectionString);
        _connection.Open();
    }

    public static DatabaseConnection Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new DatabaseConnection();
                }
                return _instance;
            }
        }
    }

    public SqliteConnection GetConnection()
    {
        return _connection;
    }
}
