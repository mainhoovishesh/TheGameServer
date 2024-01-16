using MultiplayerBackend.DatabaseCommunication.Interfaces;

namespace MultiplayerBackend.DatabaseCommunication.Services
{
    public class SQLiteDBService : IDBService
    {
        string connectionString = string.Empty;

        public void ConnectToDatabase(string connectionString)
        {
            throw new NotImplementedException();
        }

        public string GetDatabaseValues(string query)
        {
            throw new NotImplementedException();
        }

        public string SetDatabaseValues(string query)
        {
            throw new NotImplementedException();
        }
    }
}
