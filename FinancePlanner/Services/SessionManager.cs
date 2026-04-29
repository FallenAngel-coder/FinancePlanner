using FinancePlanner.Models;

namespace FinancePlanner.Services
{
    public class SessionManager
    {
        private static SessionManager _instance;
        private static readonly object _lock = new object();

        private SessionManager() { }

        public static SessionManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new SessionManager();
                    }
                    return _instance;
                }
            }
        }

        public User CurrentUser { get; set; }
    }
}
